<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb last>{{locale.fabric.plural}}</crumb>
</Bread>
<Fetch url="/resources/fabrics/list" 
    :params="{ page: props.page }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-1">{{locale.fabric.plural}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <FabricView :mat="item" 
                @click-quantity="()=> selectItem(data, item)" 
                @click-details="()=> goToItem(item)"
                class="card pad-1 mar-b-1" />
        </template>
    </Pagination>
    <Modal v-if="data.selectedItem">
        <QuantityEdit :resource="data.selectedItem"
            :errorMessage="data.quantityErrorMessage"
            @click-cancel="()=> data.selectedItem = null"
            @click-submit="(newQuantity)=> updateQuantity(data, newQuantity)" />
    </Modal>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject } from "vue"
import { nestedObjectAssign } from "@common/utils"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Modal, Pagination } from "@common/comp/layout"
import FabricView from "@/comp/mini/FabricView.vue"
import QuantityEdit from "@/comp/mini/QuantityEdit.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    page: Number,
    color: Number,
    fabrictype: Number,
    reverse: Boolean
})

function goToPage(page) {
    if (page != null) router.push(`/resources/fabrics/list?page=${page}`)
}

function goToItem(item) {
    if (item?.id) router.push(`/resources/fabrics/view/${item.id}`)
}

function selectItem (data, item) {
    if (data.selectedItem) return
    data.selectedItem = item
}

function updateQuantity (data, newQuantity) {
    let item = data.selectedItem
    axios.post({
        url: `/resources/fabrics/set-quantity/${item.id}`,
        params: { quantity: newQuantity }
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            item.inStock = Number(newQuantity)
            item.lastCheckedAt = new Date().toISOString()
            data.quantityErrorMessage = undefined
            data.selectedItem = undefined
        }
        else if (response.badFields) {
            data.quantityErrorMessage = locale.value.formatBadFields(response.badFields, l => l.resource)
        }
        else {
            data.quantityErrorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        data.quantityErrorMessage = locale.value.saveFailed.description
        console.error(error)
    })
}

function deselectItem () {
    data.selectedItem = undefined
}

</script>
