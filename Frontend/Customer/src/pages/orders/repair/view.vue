<template>
<Fetch :url="`/customer/orders/repair/view/${id}`" :cacheTTL="3" @load="onLoad" no-default 
    class-error="width-container card pad-1 mar-b-1" />
<template v-if="order">
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb :to="`/orders/repair/${tab??'list'}?page=0`">{{locale.repairOrder.plural}}</Crumb>
    <Crumb last>{{locale.order.single}} #{{order.id}}</Crumb>
</Bread>
<RepairOrderView :order="order" 
    @click-read="readNotification"
    @click-read-all="readAllNotifications"
    @click-confirm="confirmOrder"
    @click-delete="deleteOrder" />
</template>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, reactive, inject, watch } from "vue" 
import { OrderStatus } from "@common"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import RepairOrderView from "@/comp/views/RepairOrderView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number,
    tab: String
})

const order = ref(null)

function onLoad (data) {
    order.value = data
}

function readNotification (notification) {
    axios.post({
        url: "/customer/orders/repair/read-notifications",
        params: { order: props.id, id: notification.id }
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            notification.isRead = true
        }
    })
}

function readAllNotifications () {
    axios.post({
        url: "/customer/orders/repair/read-notifications",
        params: { order: props.id, last: 999999999 }
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            order.value.notifications.forEach(n => {
                n.isRead = true
            })
        }
    })
}

function confirmOrder () {
    axios.post({
        url: `/customer/orders/repair/confirm-submit/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299 && response.success) {
            order.value.status = OrderStatus.submitted
        }
    })
}

function deleteOrder () {
    axios.delete({
        url: `/customer/orders/repair/delete/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            router.push("/orders/repair/list?page=0")
        }
    })
}

</script>
