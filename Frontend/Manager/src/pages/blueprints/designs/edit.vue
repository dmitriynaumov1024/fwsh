<template>    
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <Crumb last>{{locale.design.plural}}</Crumb>
</Bread>
<div class="width-container card pad-1 mar-b-2">
    <h2 v-if="data.design?.id">{{data.design.displayName}} <span class="text-thin text-gray">#{{props.id}}</span></h2>
    <h2 v-else class="mar-b-1">{{locale.design.create}}</h2>
    <DesignEdit v-if="data.design"
        :design="data.design" 
        :photos="data.newPhotos"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage"
        @submit="submitDesign"
        @reset="resetDesign" 
        @attach-photo="attachPhoto"
        @delete-photo="deletePhoto" />
</div> 
</template>

<script setup>
import { useRouter } from "vue-router"
import { ref, reactive, inject, watch } from "vue"
import { arrayToDict, filesToFormData, jsonObjectCopy } from "@common/utils"
import { Bread, Crumb } from "@common/comp/layout"
import DesignEdit from "@/comp/edits/DesignEdit.vue"

const locale = inject("locale")
const axios = inject("axios")

const MAX_PHOTOS = 8

let designTemplate = {
    dimCompact: { }, 
    dimExpanded: { }
}

const props = defineProps({
    id: Number
})

let data = reactive({
    newPhotos: [ ],
    design: jsonObjectCopy(designTemplate),
    errorMessage: null,
    successMessage: null
})

watch(()=> props.id, getDesign, { immediate: true })

async function getDesign() {
    if (!props.id) {
        return
    }
    let { status, data: response } = await axios.get({
        url: `/manager/designs/view/${props.id}`
    })
    if (response.id) {
        designTemplate = response
        data.design = jsonObjectCopy(designTemplate)
        data.newPhotos = [ ]
    }
}

async function submitDesign() {
    let { status, data: response } = await axios.post({
        url: props.id ?
            `/manager/designs/update/${props.id}`:
            "/manager/designs/create",
        data: data.design
    })
    if (response.success) {
        data.id ??= response.id
        data.badFields = { }
        data.errorMessage = null
        data.successMessage = locale.value.changesSaved.description
        if (await submitPhotos()) {
            if (props.id) await getDesign()
            else router.push(`/blueprints/designs/view/${data.id}`)
        }
    }
    else if (response.badFields) {
        data.badFields = arrayToDict(response.badFields)
        data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.design)
    }
    else {
        data.errorMessage = locale.value.saveFailed.description
    }
}

async function submitPhotos() {
    if (data.deletePhotos) {
        await axios.post({
            url: `/manager/designs/delete-photos/${props.id}`,
            data: data.deletePhotos
        })
        data.deletePhotos = null
    }
    if (data.newPhotos) {
        let { status, data: response } = await axios.post({
            url: `/manager/designs/attach-photos/${data.id}`,
            contentType: "multipart/form-data",
            data: filesToFormData(data.newPhotos)
        })
        if (response.success) {
            data.successMessage = locale.value.changesSaved.description
            data.newPhotos = [ ]
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
        return response.success
    }
    return true
}

function resetDesign() {
    data.newPhotos = [ ]
    data.badFields = { }
    data.design = jsonObjectCopy(designTemplate)
}

function attachPhoto(photos) {
    let maxLength = (MAX_PHOTOS - data.design.photoUrls?.length)
    data.newPhotos = [...data.newPhotos, ...photos]
    if (data.newPhotos.length > maxLength) {
        data.newPhotos.length = maxLength
        data.errorMessage = locale.value.photoLimitExceeded.getDescription(MAX_PHOTOS)
    }
}

function deletePhoto(photo) {
    if (photo instanceof File) {
        data.newPhotos = data.newPhotos.filter(p => p != photo)
    }
    else {
        data.deletePhotos ??= []
        data.deletePhotos.push(photo)
        data.design.photoUrls = data.design.photoUrls.filter(p => p != photo)
    }
}

</script>