<template>
<div class="width-container card pad-1 mar-b-1">
    <div class="flex-stripe mar-b-1">
        <h2>{{locale.productionOrder.single}} #{{order.id}}</h2>
        <span class="flex-grow"></span>
        <ToggleButton v-model="showNotifications" v-slot="{ active }">
            <XIcon v-if="active" class="icon-2" />
            <NotificationIcon v-else :unread="!!(order.notifications?.find(n => !n.isRead))" class="icon-2" />
        </ToggleButton>
    </div>
    <template v-if="showNotifications">
    <h3 class="pad-05">{{locale.notification.plural}}</h3>
    <div v-if="order.notifications?.length" class="notification-list">
        <div v-for="notification of order.notifications" 
            class="pad-05 notification mar-b-1" :read="notification.isRead">
            <p>{{notification.text}}</p>
            <p class="text-gray text-right text-small">{{locale.formatDateTime(notification.createdAt)}}</p>
        </div>
    </div>
    <div v-else class="pad-1">
        <p class="text-center mar-b-1"><b>{{locale.noDataYet.title}}</b></p>
        <p class="text-center">{{locale.noDataYet.description}}</p>
    </div>
    <div class="fancy-textarea notification-input">
        <label>{{locale.notification.new}}</label>
        <textarea v-model="order.newNotificationText"></textarea>
    </div>
    <button class="button button-primary button-block pad-05"
        :disabled="!(order.newNotificationText?.length > 6)"
        @click="()=> emit('click-notify')">
        {{locale.action.send}}
    </button>
    </template>
    <template v-else>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.design.single}}</td>
            <td><button class="button link" @click="()=> emit('click-design')">{{order.design.displayName}}</button></td>
        </tr>
        <tr>
            <td>{{locale.customer.single}}</td>
            <td>
                <p class="mar-b-05">{{order.customer.name}} {{order.customer.surname}}</p>
                <p v-if="order.customer.orgName" class="text-gray mar-b-05">{{order.customer.orgName}}</p>
                <p class="text-gray mar-b-05">{{order.customer.phone}}</p>
            </td>
        </tr>
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
    "click-notify"
])

const showNotifications = ref(false)

</script>
