<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.paycheck.plural}}</Crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="mar-b-05">{{locale.paycheck.plural}}</h2>
    <div class="flex-stripe flex-pad-1">
        <template v-for="nextTab of ['list', 'archive']">
            <button v-if="nextTab==props.tab" 
                class="button button-secondary accent-weak text-strong">{{locale.common[nextTab]}}</button>
            <router-link v-else :to="`/paychecks/${nextTab}?page=0`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
        <button class="button button-primary" @click="createPaycheck">+ {{locale.action.create}}</button>
    </div>
    <div v-if="data.successMessage || data.errorMessage" class="pad-05">
        <p class="text-error">{{data.errorMessage}}</p>
        <p>{{data.successMessage}}</p>
    </div>
</div>
<Fetch :url="`/worker/paychecks/${props.tab}`"
    :params="{ page: props.page }" :cacheTTL="6"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :page="props.page" :items="data.items"
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05">
        <template v-slot:repeating="{ item }">
        <div class="card-card pad-05 mar-b-1">
            <PaycheckView :paycheck="item" class="" />
        </div>
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import PaycheckView from "@/comp/mini/PaycheckView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number,
    tab: String
})

const data = reactive({
    successMessage: null,
    errorMessage: null
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/people/paychecks/${props.tab}?page=${page}`)
}

function createPaycheck () {
    data.successMessage = null
    data.errorMessage = null
    axios.post({
        url: "/worker/paychecks/create"
    })
    .then(({ status, data: response }) => {
        if (status < 299 && response.success) {
            data.successMessage = locale.value.paycheck.created
        }
        else if (response.tryLaterAt) {
            data.errorMessage = `${locale.value.tryLater.before} ${locale.formatDateTime(response.tryLaterAt)}`
        }
        else {
            data.errorMessage = locale.value.tryLater.description
        }
    })
    .catch(error => {
        console.log(error)
        data.errorMessage = locale.value.error.description
    })
}

</script>
