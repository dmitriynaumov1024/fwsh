<template>
<div class="width-container card pad-1 mar-b-1">
    <div class="flex-stripe mar-b-1">
        <h2>{{locale.repairOrder.single}} #{{order.id}}</h2>
        <span class="flex-grow"></span>
        <ToggleButton v-model="showNotifications" v-slot="{ active }">
            <XIcon v-if="active" class="icon-2" />
            <NotificationIcon v-else :unread="!!(order.notifications?.find(n => !n.isRead))" class="icon-2" />
        </ToggleButton>
    </div>
    <template v-if="showNotifications">
    <h3 class="pad-05">{{locale.order.notifications}}</h3>
    <div v-if="order.notifications?.length" class="notification-list">
        <div v-for="notification of order.notifications" 
            class="pad-05 notification mar-b-1" 
            :read="notification.isRead"
            @click="()=> emit('click-read', notification)">
            <p>{{notification.text}}</p>
            <p class="text-gray text-right text-small">{{locale.formatDateTime(notification.createdAt)}}</p>
        </div>
    </div>
    <div v-else class="pad-1">
        <p class="text-center mar-b-1"><b>{{locale.noDataYet.title}}</b></p>
        <p class="text-center">{{locale.noDataYet.description}}</p>
    </div>
    <div v-if="!!(order.notifications?.find(n => !n.isRead))">
        <button class="button button-secondary button-block pad-05"
            @click="()=> emit('click-read-all')">
            {{locale.notification.readAll}}
        </button>
    </div>
    </template>
    <template v-else>
    <ImageGallery :items="order.photoUrls" class="mar-b-2">
        <template v-slot="{ active, item }">
            <img :src="cdnResolve(item)" :class="{ 'visible': active }">
        </template>
    </ImageGallery>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.repairOrder.description}}</td>
            <td>{{order.description}}</td>
        </tr>
        <tr>
            <td>{{locale.repairOrder.price}}</td>
            <td>{{order.price}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.repairOrder.prepayment}}</td>
            <td>{{order.payment}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.order.status}}</td>
            <td>{{locale.status[order.status]}}</td>
        </tr>
        <template v-for="actionAt of ['createdAt', 'startedAt', 'finishedAt', 'receivedAt']">
        <tr v-if="order[actionAt]">
            <td>{{locale.order[actionAt]}}</td>
            <td>{{locale.formatDateTime(order[actionAt])}}</td>
        </tr>
        </template>
    </table>
    <div class="flex-stripe">
        <button v-if="canDeleteOrder" class="button button-secondary accent-bad" 
            @click="()=> emit('click-delete')">{{locale.action.delete}}</button>
        <span class="flex-grow"></span>
        <button v-if="canConfirmOrder" class="button button-primary" 
            @click="()=> emit('click-confirm')">{{locale.action.confirm}}</button>
    </div>
    </template>
</div>
</template>

<script setup>
import { ref, computed, inject } from "vue"
import { OrderStatus } from "@common"
import { cdnResolve } from "@common/utils"
import { ToggleButton } from "@common/comp/ctrl"
import { ImageGallery } from "@common/comp/layout"
import { NotificationIcon, XIcon } from "@common/comp/icons"
import MaterialView from "../mini/MaterialView.vue"
import FabricView from "../mini/FabricView.vue"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

const showNotifications = ref(false)

const canDeleteOrder = computed (() => 
    props.order?.status == OrderStatus.unknown || 
    props.order?.status == OrderStatus.submitted
)

const canConfirmOrder = computed (() => 
    props.order?.status == OrderStatus.unknown
)

const emit = defineEmits([
    "click-read",
    "click-read-all",
    "click-confirm",
    "click-delete"
])


</script>
