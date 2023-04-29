<template>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe margin-bottom-1">
        <h2>{{locale.productionOrder.single}} #{{order.id}}</h2>
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
            class="pad-05 notification margin-bottom-1" 
            :read="notification.isRead"
            @click="()=> emit('click-read', notification)">
            <p>{{notification.text}}</p>
            <p class="text-gray text-right text-small">{{locale.formatDateTime(notification.createdAt)}}</p>
        </div>
    </div>
    <div v-else class="pad-1">
        <p class="text-center margin-bottom-1"><b>{{locale.noDataYet.title}}</b></p>
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
    <h3 class="pad-05">{{locale.productionOrder.design}}</h3>
    <table class="kvtable stripes margin-bottom-1">
        <tr>
            <td>{{locale.design.displayName}}</td>
            <td>{{order.design.displayName}}</td>
        </tr>
        <tr>
            <td>{{locale.design.type}}</td>
            <td>{{locale.furnitureTypes[order.design.type]}} ({{order.design.type}})</td>
        </tr>
        <tr>
            <td>
                <button class="button link" 
                    @click="()=> emit('click-design', order.design)">
                    {{locale.action.details}}
                </button>
            </td>
        </tr>
    </table>
    <h3 class="pad-05">{{locale.productionOrder.details}}</h3>
    <table class="kvtable stripes margin-bottom-1">
        <tr>
            <td>{{locale.productionOrder.quantity}}</td>
            <td>{{order.quantity}}</td>
        </tr>
        <tr>
            <td>{{locale.productionOrder.pricePerOne}}</td>
            <td>{{order.pricePerOne}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.productionOrder.priceTotal}}</td>
            <td><b>{{order.priceTotal}} &#8372;</b></td>
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
    </template>
</div>
</template>

<script setup>
import { ref, inject } from "vue"
import { ToggleButton } from "@common/comp/ctrl"
import { NotificationIcon, XIcon } from "@common/comp/icons"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

const emit = defineEmits([
    "click-design",
    "click-read",
    "click-read-all"
])

const showNotifications = ref(false)

</script>
