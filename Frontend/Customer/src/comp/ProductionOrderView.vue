<template>
<div class="width-container card pad-1 margin-bottom-1">
    <h2 class="margin-bottom-1">
        {{locale.productionOrder.single}} #{{order.id}}
    </h2>
    <h3 class="pad-05">{{locale.productionOrder.design}}</h3>
    <table class="kvtable stripes margin-bottom-1">
        <tr>
            <td>{{locale.design.displayName}}</td>
            <td>{{order.design.displayName}}</td>
        </tr>
        <tr>
            <td>{{locale.design.type}}</td>
            <td>{{order.design.type}}</td>
        </tr>
        <tr>
            <td><button class="button link" @click="()=> emit('click-design')">{{locale.action.details}}</button></td>
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
        <!-- <template v-for="(value, key) in order">
            <tr v-if="typeof value != 'object'">
                <td>{{ locale.productionOrder.key }}</td>
                <td>{{ value }}</td>
            </tr>
        </template> -->
    </table>
</div>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

const emit = defineEmits([
    "click-design"
])

</script>
