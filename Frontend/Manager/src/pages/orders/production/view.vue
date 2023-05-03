<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/orders">{{locale.order.plural}}</Crumb>
    <Crumb :to="`/orders/production/${props.tab??'list'}?page=0`">{{locale.productionOrder.plural}}</Crumb>
    <Crumb last v-if="order">#{{order.id}}</Crumb>
</Bread>
<Fetch :url="`/manager/orders/production/view/${id}`" :cacheTTL="4"
    @load="onLoad" no-default class-error="width-container card pad-1 mar-b-1"/>
<ProductionOrderView v-if="order" 
    :order="order" 
    @click-design="goToDesign" 
    @click-notify="notifyOrder" />
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import ProductionOrderView from "@/comp/views/ProductionOrderView.vue"

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

function goToDesign () {
    setTimeout(() => {
        router.push(`/blueprints/designs/view/${order.value.design.id}`)
    }, 200)
}

let sending = false

function notifyOrder () {
    if (sending || !(order.value.newNotificationText?.length)) return
    else sending = true
    let message = order.value.newNotificationText
    axios.post({
        url: `/manager/orders/production/notify/${props.id}`,
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
