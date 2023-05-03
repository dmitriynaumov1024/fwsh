<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb to="/resources/colors/list?page=0">{{locale.color.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <ColorEdit v-if="data.color" 
        :color="data.color"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetColor"
        @click-submit="submitColor" />
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
import ColorEdit from "@/comp/edits/ColorEdit.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let colorTemplate = { rgbCode: "#ffffff" }

const data = reactive({ 
    color: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getColor, { immediate: true })

function getColor () {
    data.color = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.color = nestedObjectCopy(colorTemplate)
        return
    }
    axios.get({
        url: `/resources/colors/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            colorTemplate = response
            data.color = nestedObjectCopy(colorTemplate)
        }
        else data.errorMessage = locale.value.common.somethingWrong
    })
    .catch(error => {
        data.errorMessage = locale.value.common.somethingWrong
    })
}

function resetColor () {
    data.color = nestedObjectCopy(colorTemplate)
    data.errorMessage = undefined
    locale.successMessage = undefined
}

function submitColor () {
    let color = data.color
    data.errorMessage = undefined
    data.successMessage = undefined
    axios.post({
        url: color.id ? 
            `/resources/colors/update/${color.id}` : 
            `/resources/colors/create`,
        data: color
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            if (response.id) data.color.id = response.id
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.color)
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = `${locale.value.common.somethingWrong}. ${locale.common.seeConsole}`
    })
}

</script>
