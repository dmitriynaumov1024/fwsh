<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb :to="`/resources/supply/list?resource=${resource ?? data.order?.itemId}`">{{locale.supplyOrder.plural}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <SupplyOrderEdit v-if="data.order" 
        :order="data.order" :canEdit="data.canEdit"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage"
        @click-save="submitOrder"
        @click-reset="resetOrder"
        @click-submit="confirmSubmit"
        @click-receive="confirmReceive"
        @click-supplier="beginSelectSupplier" />
    <div v-else>
        <p class="text-center"><b>{{locale.noData.title}}</b></p>
        <p class="text-center">{{locale.noData.description}}</p>
    </div>
</div>
<Modal v-if="data.selectingSupplier">
    <h3 class="mar-b-05">{{locale.supplier.single}}</h3>
    <div class="pad-05 height-15 scroll mar-b-1">
    <Pagination v-if="data.suppliers.items" 
        :items="data.suppliers.items"
        :page="data.suppliers.page"
        :previous="data.suppliers.previous"
        :next="data.suppliers.next"
        @click-previous="()=> { data.suppliersPage -= 1 }"
        @click-next="()=> { data.suppliersPage += 1 }">
        <template v-slot:repeating="{ item: supplier }">
            <radiobox v-model="data.suppliers.selected" :value="supplier" class="mar-b-05">
                <p>{{supplier.surname}} {{supplier.name}}</p>
                <p>{{supplier.orgName}}, {{supplier.phone}}</p>
            </radiobox>
        </template>
    </Pagination>
    </div>
    <div class="flex-stripe">
        <button class="button button-inline accent-gray" 
            @click="()=> endSelectSupplier(false)">{{locale.action.cancel}}</button>
        <span class="flex-grow"></span>
        <button v-if="data.suppliers.selected" class="button button-primary" 
            @click="()=> endSelectSupplier(true)">{{locale.action.confirm}}</button>
    </div>
</Modal>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { jsonObjectCopy, arrayToDict } from "@common/utils"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Modal, Pagination } from "@common/comp/layout"
import { Radiobox } from "@common/comp/ctrl"
import SupplyOrderEdit from "@/comp/edits/SupplyOrderEdit.vue"

const locale = inject("locale")
const axios = inject("axios")
const router = useRouter()

const props = defineProps({
    id: Number,
    resource: Number
})

let orderTemplate = { }

const data = reactive({
    order: undefined,
    badFields: { },
    errorMessage: undefined,
    successMessage: undefined,
    canEdit: false,
    selectingSupplier: false,
    suppliersPage: null,
    suppliers: { }
})

watch(()=> [ props.id, props.resource ], getOrder, { immediate: true })

function getOrder() {
    if (props.id) axios.get({
        url: `/manager/orders/supply/view/${props.id}`,
        cacheTTL: 1
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            resetOrder(response)
            data.canEdit = response.isActive || response.status == "unknown"
        }
    })
    else getResource()
}

function beginSelectSupplier() {
    data.suppliersPage ??= 0
    data.selectingSupplier = true
    if (data.suppliers.items) data.suppliers.selected = data.suppliers.items
        .find(s => s.id == data.order.item.supplier.id)
}

function endSelectSupplier (success) {
    data.selectingSupplier = false
    let supplier = data.suppliers.selected
    if (!supplier || !success) return
    data.order.supplier = supplier
    data.order.supplierId = supplier.id
}

watch(()=> data.suppliersPage, getSuppliers, { immediate: true })

function getSuppliers() {
    if (data.suppliersPage == null) return
    axios.get({
        url: "/manager/suppliers/list",
        params: { page: data.suppliersPage },
        cacheTTL: 30
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.suppliers = response
            data.suppliers.selected = data.suppliers.items
                .find(s => s.id == data.order.item.supplier.id)
        }
    })
}

function getResource() {
    axios.get({
        url: `/resources/any/view/${props.resource}`,
        cacheTTL: 10
    })
    .then(({ status, data: response }) => {
        if (status < 299) { 
            resetOrder({
                item: response,
                itemId: response.id,
                externalId: response.externalId,
                supplier: response.supplier,
                supplierId: response.supplierId
            })
            data.canEdit = true
        }
    })
}

function resetOrder (template) {
    if (template) orderTemplate = template
    data.order = jsonObjectCopy(orderTemplate)
}

function submitOrder() {
    let promise = axios.post({
        url: props.id ? 
            `/manager/orders/supply/update/${props.id}` :
            `/manager/orders/supply/create`,
        data: data.order
    })
    promise.then(({ status, data: response }) => {
        if (status < 299) {
            data.errorMessage = undefined
            data.badFields = { }
            if (props.id) getOrder()
            else if (response.id) router.push(`/resources/supply/edit/${response.id}`)
            else data.errorMessage = locale.value.error.description
        }
        else if (response.badFields?.length) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.supplyOrder)
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
    return promise
}

function confirmSubmit() {
    axios.post({
        url: `/manager/orders/supply/confirm-submit/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            getOrder()
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function confirmReceive() {
    axios.post({
        url: `/manager/orders/supply/update/${props.id}`,
        data: data.order
    })
    .then(()=> {
        axios.post({
            url: `/manager/orders/supply/confirm-receive/${props.id}`
        })
        .then(({ status, data: response }) => {
            if (status < 299) {
                data.order.status = OrderStatus.unknown
                setTimeout(getOrder, 1000)
            }
        })
        .catch(error => {
            console.error(error)
            data.errorMessage = locale.value.error.description
        })
    })
}

</script>
