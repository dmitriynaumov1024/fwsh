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
            <td><b>{{order.price}} &#8372;</b></td>
        </tr>
        <tr>
            <td>{{locale.order.status}}</td>
            <td clickable @click="beginEditStatus">{{locale.status[order.status]}}</td>
        </tr>
        <tr>
            <td>{{locale.order.isActive}}</td>
            <td><checkbox :checked="order.isActive" 
                @click="()=>emit('click-active', !order.isActive)">
                {{locale.setActiveResult[order.isActive]}}</checkbox></td>
        </tr>
        <template v-for="actionAt of ['createdAt', 'startedAt', 'finishedAt', 'receivedAt']">
        <tr v-if="order[actionAt]">
            <td>{{locale.order[actionAt]}}</td>
            <td>{{locale.formatDateTime(order[actionAt])}}</td>
        </tr>
        </template>
    </table>
    <div class="flex-stripe">
        <h3 class="flex-grow">{{locale.task.plural}}</h3>
        <button v-if="order.status == OrderStatus.submitted" 
            @click="()=> emit('click-createtasks')" class="button button-primary">{{locale.action.create}}</button>
        <router-link v-else-if="order.status != OrderStatus.unknown" 
            :to="`/tasks/production/list?order=${order.id}`"
            class="button button-primary">{{locale.action.details}}</router-link>
    </div>
    </template>
</div>
<Modal v-if="showEditStatus">
    <h3 class="mar-b-1">{{locale.order.status}}</h3>
    <div v-for="(status, key) in OrderStatus" class="mar-b-05">
        <radiobox :checked="status == order.status"
            @click="()=> endEditStatus(status)">
            {{locale.status[status]}}
        </radiobox>
    </div>
    <div class="mar-b-2"></div>
    <div>
        <button class="button button-inline accent-gray" 
            @click="()=> endEditStatus()">{{locale.action.cancel}}
        </button>
    </div>
</Modal>
</template>

<script setup>
import { OrderStatus } from "@common"
import { ref, reactive, inject } from "vue"
import { ToggleButton, Checkbox, Radiobox } from "@common/comp/ctrl"
import { NotificationIcon, XIcon } from "@common/comp/icons"
import { ImageGallery, Modal } from "@common/comp/layout"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

const emit = defineEmits([
    "click-design",
    "click-notify",
    "click-status",
    "click-active",
    "click-createtasks"
])

const showNotifications = ref(false)

const showEditStatus = ref(false)

function beginEditStatus() {
    showEditStatus.value = true
}

function endEditStatus(status) {
    console.log(status)
    if (status) emit("click-status", status)
    showEditStatus.value = false
}

</script>
