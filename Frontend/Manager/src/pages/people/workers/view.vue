<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/people">{{locale.person.plural}}</Crumb>
    <Crumb to="/people/workers/list?page=0">{{locale.worker.plural}}</Crumb>
    <Crumb last v-if="data.worker?.id">#{{data.worker.id }}</Crumb>
</Bread>
<div class="width-container card pad-1">
    <WorkerView v-if="data.worker" :worker="data.worker"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage"
        @click-create-paycheck="createPaycheck"
        @click-create-bonus="createBonus"
        @click-confirm-payment="confirmPayment" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Bread, Crumb } from "@common/comp/layout"
import WorkerView from "@/comp/views/WorkerView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    id: Number
})

const data = reactive({ 
    worker: undefined,
    errorMessage: undefined,
    successMessage: undefined,
    badFields: { },
})

watch(() => props.id, getWorker, { immediate: true })

function getWorker () {
    data.worker = undefined
    axios.get({
        url: `/manager/workers/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.worker = response
        }
        else data.errorMessage = locale.value.error.description
    })
    .catch(error => {
        data.errorMessage = locale.value.error.description
    })
}

function createPaycheck () {
    axios.post({
        url: "/manager/paychecks/create",
        params: { workerId: props.id }
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            setTimeout(getWorker, 120)
            data.errorMessage = undefined
            data.successMessage = locale.value.paycheck.created
        }
        else if (response.tryLaterAt) {
            data.errorMessage = `${locale.value.tryLater.before} ${locale.value.formatDateTime(response.tryLaterAt)}`
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

function createBonus (amount) {
    amount = Number(amount)
    if (Number.isNaN(amount)) amount = 0
    axios.post({
        url: "/manager/paychecks/create-bonus",
        params: { workerId: props.id, amount: amount }
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            setTimeout(getWorker, 120)
            data.errorMessage = undefined
            data.successMessage = locale.value.paycheck.created
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

function confirmPayment (paycheck) {
    axios.post({
        url: "/manager/paychecks/confirm-payment",
        params: { workerId: paycheck.workerId, paycheckId: paycheck.id }
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            setTimeout(getWorker, 120)
            data.errorMessage = undefined
            data.successMessage = locale.value.paycheck.paid
        }
        else {
            data.errorMessage = locale.value.error.description
        }
    })
    .catch(error => {
        console.log(error)
        data.errorMessage = locale.value.error.description
    })
}

</script>
