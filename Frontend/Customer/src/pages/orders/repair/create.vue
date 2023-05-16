<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/orders/repair/list?page=0">{{locale.repairOrder.plural}}</Crumb>
    <Crumb last>{{locale.action.makeOrder}}</Crumb>
</Bread>
<div class="width-container card pad-1">
    <h2 class="mar-b-1">{{locale.repairOrder.create}}</h2>
    <textbox v-model="data.order.description" :invalid="data.badFields.description">
        {{locale.repairOrder.description}}
    </textbox>
    <div class="fancy-group">
        <header>{{locale.photo.plural}}</header>
        <main>
            <div class="preview-photo-gallery">
                <div v-for="photo of data.photos">
                    <img :src="URL.createObjectURL(photo)" @load="URL.revokeObjectURL(photo)">
                </div>
            </div>
            <div>
                <input type="file" multiple id="input-photos" ref="photoInput" 
                    @change="photoInputChanged" class="hidden">
                <label for="input-photos" 
                    class="button button-secondary button-block accent-gray">
                    + {{locale.action.addPhotos}}
                </label>
            </div>
        </main>
    </div>
    <div class="mar-b-2">
        <span class="text-error">{{data.errorMessage}}</span>
    </div>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-inline accent-bad" @click="resetOrder">{{locale.action.reset}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary" @click="submitOrder">{{locale.action.submit}}</button>
    </div>
</div>
</template>

<script setup>
import { arrayToDict, nestedObjectCopy } from "@common/utils"
import { useRouter } from "vue-router"
import { reactive, ref, inject } from "vue"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import { Inputbox, Textbox } from "@common/comp/ctrl"

const URL = window.URL
const router = useRouter()
const storage = inject("storage")
const axios = inject("axios")
const locale = inject("locale")

const orderTemplate = {
    description: "..."
}

const data = reactive({
    order: nestedObjectCopy(orderTemplate),
    photos: [ ],
    badFields: { },
    errorMessage: null
})

const photoInput = ref(null)

function photoInputChanged() {
    data.photos = [ ...(data.photos), ...photoInput.value.files ]
}

function resetOrder() {
    data.order = nestedObjectCopy(orderTemplate)
    data.photos = [ ]
    data.badFields = { }
    data.errorMessage = null
}

function submitOrder() {
    axios.post({
        url: "/customer/orders/repair/create",
        data: data.order
    })
    .then(({ status, data: response }) => {
        if (response.success && response.id) {
            attachPhotos(response.id)
            .then(() => {
                router.replace(`/orders/repair/view/${response.id}`)
            })
            .catch(error => {
                data.errorMessage = locale.value.error.description
            })
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.repairOrder)
        }
        else {
            data.errorMessage = locale.value.error.description
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function attachPhotos (id) {
    let formData = new FormData()
    for (const photo of data.photos) formData.append("files", photo)
    return axios.post({
        url: `/customer/orders/repair/attach-photos/${id}`,
        contentType: "multipart/form-data",
        data: formData
    })
}

</script>
