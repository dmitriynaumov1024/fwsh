<template>
<Fetch :url="`/customer/orders/production/view/${id}`" :cacheTTL="3" @load="onLoad" no-default 
    class-error="width-container card pad-1 mar-b-1" />
<template v-if="order">
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb :to="`/orders/production/${tab}?page=0`">{{locale.productionOrder.plural}}</Crumb>
    <Crumb last>{{locale.order.single}} #{{order.id}}</Crumb>
</Bread>
<ProductionOrderView :order="order" 
    @click-design="goToDesign"
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
import ProductionOrderView from "@/comp/views/ProductionOrderView.vue"

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

function goToDesign (design) {
    setTimeout(() => {
        router.push(`/catalog/designs/view/${design.id}`)
    }, 200)
}

function readNotification (notification) {
    axios.post({
        url: "/customer/orders/production/read-notifications",
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
        url: "/customer/orders/production/read-notifications",
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
        url: `/customer/orders/production/confirm-submit/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299 && response.success) {
            order.value.status = OrderStatus.submitted
        }
    })
}

function deleteOrder () {
    axios.delete({
        url: `/customer/orders/production/delete/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            router.push("/orders/production/list?page=0")
        }
    })
}

</script>
