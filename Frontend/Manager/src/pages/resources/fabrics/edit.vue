<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/fabrics/list?page=0">{{locale.fabric.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <FabricEdit v-if="data.fabric" 
        :mat="data.fabric"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-supplier="beginSelectSupplier"
        @click-reset="resetFabric"
        @click-submit="submitFabric" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>
<Modal v-if="data.selectingSupplier && data.suppliers">
    <SupplierSelect :suppliers="data.suppliers" 
        :selection="data.fabric.supplier"
        @click-submit="endSelectSupplier"
        @click-cancel="endSelectSupplier" />
</Modal>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { arrayToDict, nestedObjectCopy } from "@common/utils"
import { Bread, Crumb, Modal } from "@common/comp/layout"
import FabricEdit from "@/comp/edits/FabricEdit.vue"
import SupplierSelect from "@/comp/mini/SupplierSelect.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let fabricTemplate = { measureUnit: "m2" }

const data = reactive({ 
    fabric: undefined,
    selectingSupplier: false,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getFabric, { immediate: true })

function getFabric () {
    data.fabric = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.fabric = nestedObjectCopy(fabricTemplate)
        return
    }
    axios.get({
        url: `/resources/fabrics/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            fabricTemplate = response
            data.fabric = nestedObjectCopy(fabricTemplate)
        }
        else data.errorMessage = locale.value.error.description
    })
    .catch(error => {
        data.errorMessage = locale.value.error.description
    })
}

function resetFabric () {
    data.fabric = nestedObjectCopy(fabricTemplate)
    data.errorMessage = undefined
    data.successMessage = undefined
}

function submitFabric () {
    let fabric = data.fabric
    data.errorMessage = undefined
    data.successMessage = undefined
    data.badFields = { }
    axios.post({
        url: fabric.id ? 
            `/resources/fabrics/update/${fabric.id}` : 
            `/resources/fabrics/create`,
        data: { ...fabric }
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            fabricTemplate = nestedObjectCopy(data.fabric)
            if (response.id) data.fabric.id = response.id
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.resource)
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function beginSelectSupplier() {
    data.selectingSupplier = true
    if (!data.suppliers) {
        axios.get({
            url: "/manager/suppliers/list",
            params: { page: 0 },
            cacheTTL: 20
        })
        .then(({ status, data: response }) => {
            data.suppliers = response.items
        })
    }
}

function endSelectSupplier (supplier) {
    data.selectingSupplier = false
    if (supplier) {
        data.fabric.supplier = supplier
        data.fabric.supplierId = supplier.id
    }
}

</script>
