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
            <router-link v-else :to="`/people/paychecks/${nextTab}?page=0`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
        <router-link to="/people/workers/list?page=0" 
            class="button button-primary accent-weak">{{locale.worker.plural}}</router-link>
    </div>
</div>
<Fetch :url="`/manager/paychecks/${props.tab}`"
    :params="{ page: props.page}" :cacheTTL="6"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :page="props.page" :items="data.items"
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05">
        <template v-slot:repeating="{ item }">
        <div v-if="props.tab == 'list'" class="card-card pad-05 mar-b-1">
            <PersonView :person="item" 
                class="mar-b-1" clickable
                @click="goToItem(item)" />
            <PaycheckView v-for="paycheck of item.paychecks" 
                :paycheck="paycheck" :canPay="!paycheck.isReceived"
                class="border-left-2 mar-b-05"
                @click-pay="()=> confirmPayment(paycheck)" />
        </div>
        <div v-else class="card-card pad-05 mar-b-1">
            <PaycheckView :workerId="true"
                :paycheck="item" 
                class="" />
        </div>
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import PersonView from "@/comp/mini/PersonView.vue"
import PaycheckView from "@/comp/mini/PaycheckView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number,
    tab: String
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/people/paychecks/${props.tab}?page=${page}`)
}

function goToItem (item) {
    if (item.id) router.push(`/people/workers/view/${item.id}`)
}

function confirmPayment (paycheck) {
    axios.post({
        url: "/manager/paychecks/confirm-payment",
        params: { workerId: paycheck.workerId, paycheckId: paycheck.id }
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            paycheck.isReceived = true
        }
    })
    .catch(error => {
        console.log(error)
    })
}

</script>
