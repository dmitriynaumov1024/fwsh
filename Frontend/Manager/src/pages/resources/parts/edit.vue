<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/parts/list?page=0">{{locale.part.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <PartEdit v-if="data.part" 
        :part="data.part"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetPart"
        @click-submit="submitPart" />
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
import PartEdit from "@/comp/edits/PartEdit.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let partTemplate = { 
    item: { }
}

const data = reactive({ 
    part: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getPart, { immediate: true })

function getPart () {
    data.part = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.part = nestedObjectCopy(partTemplate)
        return
    }
    axios.get({
        url: `/resources/parts/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            partTemplate = response
            data.part = nestedObjectCopy(partTemplate)
        }
        else data.errorMessage = locale.value.common.somethingWrong
    })
    .catch(error => {
        data.errorMessage = locale.value.common.somethingWrong
    })
}

function resetPart () {
    data.part = nestedObjectCopy(partTemplate)
    data.errorMessage = undefined
    locale.successMessage = undefined
}

function submitPart () {
    let part = data.part
    data.errorMessage = undefined
    data.successMessage = undefined
    data.badFields = { }
    axios.post({
        url: part.id ? 
            `/resources/parts/update/${part.id}` : 
            `/resources/parts/create`,
        data: { ...part, ...part.item }
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            if (response.id) data.part.id = response.id
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
