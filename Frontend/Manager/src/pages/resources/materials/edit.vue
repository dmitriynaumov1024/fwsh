<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/materials/list?page=0">{{locale.material.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <MaterialEdit v-if="data.material" 
        :material="data.material"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetMaterial"
        @click-submit="submitMaterial" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { arrayToDict, nestedObjectCopy } from "@common/utils"
import { Bread, Crumb } from "@common/comp/layout"
import MaterialEdit from "@/comp/edits/MaterialEdit.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let materialTemplate = { 
    item: { }
}

const data = reactive({ 
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
        else data.errorMessage = locale.value.common.somethingWrong
    })
    .catch(error => {
        data.errorMessage = locale.value.common.somethingWrong
    })
}

function resetMaterial () {
    data.material = nestedObjectCopy(materialTemplate)
    data.errorMessage = undefined
    locale.successMessage = undefined
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
        data: { ...material, ...material.item }
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            if (response.id) data.material.id = response.id
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.resource)
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = `${locale.value.common.somethingWrong}. ${locale.common.seeConsole}`
    })
}

</script>
