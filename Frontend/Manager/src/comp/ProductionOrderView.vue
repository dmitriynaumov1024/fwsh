<template>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe margin-bottom-1">
        <h2>{{locale.productionOrder.single}} #{{order.id}}</h2>
        <span class="flex-grow"></span>
        <NotificationButton class="icon-2"
            :active="showNotifications"
            @click="toggleNotifications" />
    </div>
    <template v-if="showNotifications">
    <h3 class="pad-05">{{locale.notification.plural}}</h3>
    <div v-if="order.notifications?.length" class="notification-list">
        <div v-for="notification of order.notifications" 
            class="pad-05 notification margin-bottom-1" :read="notification.isRead">
            <p>{{notification.text}}</p>
            <p class="text-gray text-right text-small">{{locale.formatDateTime(notification.createdAt)}}</p>
        </div>
    </div>
    <div v-else class="pad-1">
        <p class="text-center margin-bottom-1"><b>{{locale.noDataYet.title}}</b></p>
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
    <table class="kvtable stripes margin-bottom-1">
        <tr>
            <td>{{locale.design.single}}</td>
            <td><button class="button link" @click="()=> emit('click-design')">{{order.design.displayName}}</button></td>
        </tr>
        <tr>
            <td>{{locale.customer.single}}</td>
            <td>
                <p class="margin-bottom-05">{{order.customer.name}} {{order.customer.surname}}</p>
                <p v-if="order.customer.orgName" class="text-gray margin-bottom-05">{{order.customer.orgName}}</p>
                <p class="text-gray margin-bottom-05">{{order.customer.phone}}</p>
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
import NotificationButton from "@/comp/ctrl/Notification.vue"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

const emit = defineEmits([
    "click-design",
    "click-notify"
])

const showNotifications = ref(false)

function toggleNotifications() {
    showNotifications.value = !(showNotifications.value)
}

</script>
