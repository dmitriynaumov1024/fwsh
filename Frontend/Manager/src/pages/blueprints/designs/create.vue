<template>    
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <Crumb last>{{locale.design.plural}}</Crumb>
</Bread>
<div class="width-container card pad-1 mar-b-2">
    <h2 class="mar-b-1">{{locale.design.create}}</h2>
    <div v-if="data.success">
        <p>{{locale.design.creationMessage}}</p>
        <router-link :to="`/blueprints/designs/view/${data.id}`" class="link">
            {{locale.action.details}}
        </router-link>
    </div>
    <DesignEdit v-else 
        :design="data.design" 
        :photos="data.photos"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        @submit="createDesign"
        @reset="resetDesign" 
        @attach-photo="attachPhoto"
        @delete-photo="deletePhoto" />
</div> 
</template>

<script setup>
import { useRouter } from "vue-router"
import { ref, reactive, inject } from "vue"
import { arrayToDict } from "@common/utils"
import { Bread, Crumb } from "@common/comp/layout"
import DesignEdit from "@/comp/edits/DesignEdit.vue"

const locale = inject("locale")
const axios = inject("axios")

function designTemplate() {
    return {
        dimCompact: { }, 
        dimExpanded: { }
    }
}

let data = reactive({
    photos: [ ],
    design: designTemplate()
})

function createDesign() {
    console.log(data.design)
    axios.post({
        url: "/manager/designs/create",
        data: data.design
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            data.id = response.id
            attachPhotosToDesign()
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, locale => locale.design)
        }
    })
}

function attachPhotosToDesign() {
    let formData = new FormData()
    for (const photo of data.photos) formData.append("files", photo)
    axios.post({
        url: `/manager/designs/attach-photos/${data.id}`,
        contentType: "multipart/form-data",
        data: formData
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            data.success = true
        }
    })
}

function resetDesign() {
    data.photos = [ ]
    data.badFields = { }
    data.design = designTemplate()
}

function attachPhoto(photos) {
    data.photos = [...data.photos, ...photos]
}

function deletePhoto() {

}

</script>