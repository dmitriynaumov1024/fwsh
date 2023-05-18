<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/orders">{{locale.order.plural}}</Crumb>
    <Crumb :to="`/orders/repair/${props.tab??'list'}?page=0`">{{locale.repairOrder.plural}}</Crumb>
    <Crumb last v-if="order">#{{order.id}}</Crumb>
</Bread>
<Fetch :url="`/manager/orders/repair/view/${id}`" :cacheTTL="4"
    @load="onLoad" no-default class-error="width-container card pad-1 mar-b-1"/>
<RepairOrderView v-if="order" :order="order" 
    @click-status="setStatus"
    @click-active="setActive"
    @click-notify="notifyOrder" />
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import RepairOrderView from "@/comp/views/RepairOrderView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    id: Number
})

const order = ref(null)

function onLoad (data) {
    order.value = data
}

function setStatus (newStatus) {
    axios.post({
        url: `/manager/orders/repair/set-status/${props.id}`,
        params: { status: newStatus }
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            order.value.status = newStatus
        }
        else {
            order.value.errorMessage = locale.value.saveFailed
        }
    })
}

function setActive (active) {
    axios.post({
        url: `/manager/orders/repair/set-active/${props.id}`,
        params: { active: active }
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            order.value.isActive = active
        }
        else {
            order.value.errorMessage = locale.value.saveFailed
        }
    })
}


let sending = false

function notifyOrder () {
    if (sending || !(order.value.newNotificationText?.length)) return
    else sending = true
    let message = order.value.newNotificationText
    axios.post({
        url: `/manager/orders/repair/notify/${props.id}`,
        data: message
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            order.value.notifications.push({
                createdAt: new Date().toISOString(),
                text: message,
                isRead: false
            })
            sending = false
            order.value.newNotificationText = ""
        }
    })
}

</script>
