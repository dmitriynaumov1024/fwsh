<template>
<div v-if="part.id" class="flex-stripe mar-b-1">
    <h2 class="flex-grow">{{locale.part.single}} <span class="text-thin text-gray">#{{part.id}}</span></h2>
    <router-link :to="`/resources/supply/list?page=0&resource=${part.id}`" 
        class="button button-secondary">{{locale.supplyOrder.plural}}</router-link>
</div>
<h2 v-else class="mar-b-1">{{locale.part.create}}</h2>

<inputbox type="text" v-model="part.name" :invalid="badFields.name">
    {{locale.resource.name}}</inputbox>
<inputbox type="text" v-model="part.externalId" :invalid="badFields.externalId">
    {{locale.resource.externalId}}</inputbox>
<textbox v-model="part.description" :invalid="badFields.description">
    {{locale.resource.description}}</textbox>
<inputbox type="number" v-model="part.pricePerUnit" :invalid="badFields.pricePerUnit">
    {{locale.resource.pricePerUnit}}, &#8372;</inputbox>
<inputbox type="number" v-model="part.inStock" :invalid="badFields.inStock">
    {{locale.resource.inStock}}</inputbox>
<inputbox type="number" v-model="part.normalStock" :invalid="badFields.normalStock">
    {{locale.resource.normalStock}}</inputbox>
<inputbox type="number" v-model="part.refillPeriodDays" :invalid="badFields.refillPeriodDays">
    {{locale.resource.refillPeriodDays}}</inputbox>

<groupbox clickable @click="()=> emit('click-supplier')">
    <template v-slot:header>
        {{locale.supplier.single}}
    </template>
    <template v-if="part.supplier">
        <p>{{part.supplier.surname}} {{part.supplier.name}}</p>
        <p>{{part.supplier.orgName}}, {{part.supplier.phone}}</p>
    </template>
    <template v-else>
        <p>{{locale.common.notSelected}}</p>
    </template>
</groupbox>

<inputbox v-if="part.createdAt" disabled :value="locale.formatDateTime(part.createdAt)">
    {{locale.resource.createdAt}}</inputbox>
<inputbox v-if="part.lastRefilledAt" disabled :value="locale.formatDateTime(part.lastRefilledAt)">
    {{locale.resource.lastRefilledAt}}</inputbox>
<inputbox v-if="part.lastCheckedAt" disabled :value="locale.formatDateTime(part.lastCheckedAt)">
    {{locale.resource.lastCheckedAt}}</inputbox>

<div class="mar-b-2">
    <p class="text-error">{{errorMessage}}</p>
    <p>{{successMessage}}</p>
</div>

<div class="flex-stripe">
    <button class="button button-secondary accent-gray" @click="emit('click-reset')">{{locale.action.reset}}</button>
    <span class="flex-grow"></span>
    <button class="button button-primary" @click="emit('click-submit')">{{locale.action.save}}</button>
</div>
</template>

<script setup>
import { inject } from "vue"
import { Groupbox, Inputbox, Textbox, Radiobox } from "@common/comp/ctrl"

const locale = inject("locale") 

const props = defineProps({
    part: Object,
    successMessage: String,
    errorMessage: String,
    badFields: {
        type: Object,
        default: { }
    }
})

const emit = defineEmits([
    "click-supplier",
    "click-reset",
    "click-submit"
])

</script>
