<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/materials/list?page=0">{{locale.material.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <MaterialEdit v-if="data.material" 
        :mat="data.material"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-supplier="beginSelectSupplier"
        @click-reset="resetMaterial"
        @click-submit="submitMaterial" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>
<Modal v-if="data.selectingSupplier && data.suppliers">
    <SupplierSelect :suppliers="data.suppliers" 
        :selection="data.material.supplier"
        @click-submit="endSelectSupplier"
        @click-cancel="endSelectSupplier" />
</Modal>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { arrayToDict, nestedObjectCopy } from "@common/utils"
import { Bread, Crumb, Modal } from "@common/comp/layout"
import MaterialEdit from "@/comp/edits/MaterialEdit.vue"
import SupplierSelect from "@/comp/mini/SupplierSelect.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let materialTemplate = { }

const data = reactive({ 
    selectingSupplier: false,
    material: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getMaterial, { immediate: true })

function getMaterial () {
    data.material = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.material = nestedObjectCopy(materialTemplate)
        return
    }
    axios.get({
        url: `/resources/materials/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            materialTemplate = response
            data.material = nestedObjectCopy(materialTemplate)
        }
        else data.errorMessage = locale.value.error.description
    })
    .catch(error => {
        data.errorMessage = locale.value.error.description
    })
}

function resetMaterial () {
    data.material = nestedObjectCopy(materialTemplate)
    data.errorMessage = undefined
    data.successMessage = undefined
}

function submitMaterial () {
    let material = data.material
    data.errorMessage = undefined
    data.successMessage = undefined
    data.badFields = { }
    axios.post({
        url: material.id ? 
            `/resources/materials/update/${material.id}` : 
            `/resources/materials/create`,
        data: { ...material }
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            materialTemplate = nestedObjectCopy(data.material)
            if (response.id) data.material.id = response.id
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
        data.material.supplier = supplier
        data.material.supplierId = supplier.id
    }
}

</script>
