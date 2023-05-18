<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/people">{{locale.person.plural}}</crumb>
    <crumb to="/people/suppliers/list?page=0">{{locale.supplier.plural}}</crumb>
    <crumb last v-if="props.id">#{{props.id}}</crumb>
</Bread>
<div class="width-container card pad-1">
    <PersonEdit v-if="data.supplier" type="supplier"
        :person="data.supplier"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetSupplier"
        @click-submit="submitSupplier" />
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
import PersonEdit from "@/comp/edits/PersonEdit.vue"

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

let pTemplate = { }

const data = reactive({ 
    supplier: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getSupplier, { immediate: true })

function getSupplier () {
    data.supplier = undefined
    data.errorMessage = undefined
    data.successMessage = undefined
    if (!props.id) {
        data.supplier = nestedObjectCopy(pTemplate)
        return
    }
    axios.get({
        url: `/manager/suppliers/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            pTemplate = response
            data.supplier = nestedObjectCopy(pTemplate)
        }
        else data.errorMessage = locale.value.error.description
    })
    .catch(error => {
        data.errorMessage = locale.value.error.description
    })
}

function resetSupplier () {
    data.supplier = nestedObjectCopy(pTemplate)
    data.errorMessage = undefined
    data.successMessage = undefined
}

function submitSupplier () {
    let supplier = data.supplier
    data.errorMessage = undefined
    data.successMessage = undefined
    data.badFields = { }
    axios.post({
        url: supplier.id ? 
            `/manager/suppliers/update/${supplier.id}` : 
            `/manager/suppliers/create`,
        data: supplier
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            data.successMessage = locale.value.changesSaved.description
            if (response.id) data.supplier.id = response.id
            pTemplate = nestedObjectCopy(data.supplier)
        }
        else if (response.badFields) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields, l => l.profile)
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

</script>
