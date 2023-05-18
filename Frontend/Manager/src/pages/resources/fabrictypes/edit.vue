<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/fabrictypes/list?page=0">{{locale.fabricType.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <FabricTypeEdit v-if="data.ftype" 
        :ftype="data.ftype"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetFabricType"
        @click-submit="submitFabricType" />
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
import FabricTypeEdit from "@/comp/edits/FabricTypeEdit.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let ftypeTemplate = { }

const data = reactive({ 
    color: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getFabricType, { immediate: true })

function getFabricType () {
    data.ftype = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.ftype = { }
        return
    }
    axios.get({
        url: `/resources/fabrictypes/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            ftypeTemplate = response
            data.ftype = nestedObjectCopy(ftypeTemplate)
        }
        else data.errorMessage = locale.value.error.description
    })
    .catch(error => {
        data.errorMessage = locale.value.error.description
    })
}

function resetFabricType () {
    data.ftype = nestedObjectCopy(ftypeTemplate)
    data.errorMessage = undefined
    data.successMessage = undefined
}

function submitFabricType () {
    let ftype = data.ftype
    data.errorMessage = undefined
    data.successMessage = undefined
    axios.post({
        url: ftype.id ? 
            `/resources/fabrictypes/update/${ftype.id}` : 
            `/resources/fabrictypes/create`,
        data: ftype
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            if (response.id) data.ftype.id = response.id
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.fabricType)
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

</script>
