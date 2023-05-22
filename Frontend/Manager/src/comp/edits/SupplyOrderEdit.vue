<template>
<h2>{{locale.order.single}} <span v-if="order.id" class="text-thin text-gray">#{{order.id}}</span></h2>

<groupbox>
    <template v-slot:header>
        {{locale.resource.single}}
    </template>
    <p><b>{{order.item.name}}</b></p>
    <p>{{locale.resource.pricePerUnit}}: {{order.item.pricePerUnit}} &#8372;</p>
    <p>{{locale.resource.inStock}}: {{order.item.inStock}} / {{order.item.normalStock}} <template v-if="order.item.measureUnit">{{locale.measureUnits[order.item.measureUnit]}}</template></p>
    <p>{{locale.resource.lastRefilledAt}}: {{locale.formatDate(order.item.lastRefilledAt)}}</p>
</groupbox>

<groupbox clickable @click="()=> emit('click-supplier')">
    <template v-slot:header>
        {{locale.supplier.single}}
    </template>
    <template v-if="order.supplier">
        <p>{{order.supplier.surname}} {{order.supplier.name}}</p>
        <p>{{order.supplier.orgName}}, {{order.supplier.phone}}</p>
    </template>
    <template v-else>
        <p>{{locale.common.notSelected}}</p>
    </template>
</groupbox>

<inputbox type="text" v-model="order.externalId">
    {{locale.resource.externalId}}</inputbox>

<inputbox :disabled="!canEdit" type="number" 
    v-model="order.expectQuantity" :invalid="badFields.expectQuantity">
    {{locale.supplyOrder.expectQuantity}}<template v-if="order.item.measureUnit">, {{locale.measureUnits[order.item.measureUnit]}}</template></inputbox>
<inputbox :disabled="!canEdit" type="number" 
    v-model="order.expectPricePerUnit" :invalid="badFields.expectPricePerUnit">
    {{locale.supplyOrder.expectPrice}}, &#8372; 
    <template v-if="order.item.measureUnit">/ {{locale.measureUnits[order.item.measureUnit]}}</template></inputbox>

<inputbox :disabled="!canEdit || !order.isActive" type="number" 
    v-model="order.actualQuantity" :invalid="badFields.actualQuantity">
    {{locale.supplyOrder.actualQuantity}}<template v-if="order.item.measureUnit">, {{locale.measureUnits[order.item.measureUnit]}}</template></inputbox>
<inputbox :disabled="!canEdit || !order.isActive" type="number" 
    v-model="order.actualPricePerUnit" :invalid="badFields.actualPricePerUnit">
    {{locale.supplyOrder.actualPrice}}, &#8372; 
    <template v-if="order.item.measureUnit">/ {{locale.measureUnits[order.item.measureUnit]}}</template></inputbox>

<inputbox disabled v-if="order.status" :value="locale.status[order.status]">
    {{locale.order.status}}</inputbox>
<inputbox disabled v-if="order.createdAt" :value="locale.formatDateTime(order.createdAt)">
    {{locale.order.createdAt}}</inputbox>
<inputbox disabled v-if="order.receivedAt" :value="locale.formatDateTime(order.receivedAt)">
    {{locale.order.receivedAt}}</inputbox>

<div class="mar-b-2">
    <p class="text-error">{{errorMessage}}</p>
    <p>{{successMessage}}</p>
</div>

<div class="flex-stripe mar-b-1">
    <button class="button button-secondary accent-weak text-gray" 
        @click="()=> emit('click-reset')">{{locale.action.reset}}</button>
    <span class="flex-grow"></span>
    <button class="button button-primary" 
        @click="()=> emit('click-save')">{{locale.action.save}}</button>
</div>
<div class="border-bottom mar-b-1"></div>

<div v-if="order.status == OrderStatus.unknown" class="flex-stripe">
    <b class="flex-grow">{{locale.supplyOrder.submitOrder}}</b>
    <button class="button button-secondary" 
        @click="()=> emit('click-submit')">{{locale.action.confirm}}</button>
</div>
<div v-if="order.status == OrderStatus.submitted" class="flex-stripe">
    <b class="flex-grow">{{locale.supplyOrder.closeOrder}}</b>
    <button class="button button-secondary"
        @click="()=> emit('click-receive')">{{locale.action.confirm}}</button>
</div>
</template>

<script setup>
import { OrderStatus } from "@common"
import { inject } from "vue"
import { Inputbox, Groupbox } from "@common/comp/ctrl"

const locale = inject("locale")

const props = defineProps({
    order: Object,
    canEdit: Boolean,
    badFields: Object,
    errorMessage: String,
    successMessage: String
})

const emit = defineEmits([
    "click-reset",
    "click-save",
    "click-submit",
    "click-receive",
    "click-supplier"
])

</script>
