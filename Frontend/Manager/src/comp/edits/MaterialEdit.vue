<template>
<h2 v-if="material.id" class="mar-b-1">{{locale.material.single}} #{{material.id}}</h2>
<h2 v-else class="mar-b-1">{{locale.material.create}}</h2>

<inputbox disabled :value="material.id" >
    {{locale.common.id}}</inputbox>
<inputbox type="text" v-model="material.item.name" :invalid="badFields.name">
    {{locale.resource.name}}</inputbox>
<inputbox type="text" v-model="material.externalId" :invalid="badFields.externalId">
    {{locale.resource.externalId}}</inputbox>
<textbox v-model="material.item.description" :invalid="badFields.description">
    {{locale.resource.description}}</textbox>
<groupbox :invalid="badFields.measureUnit">
    <template v-slot:header>
        {{locale.resource.measureUnit}}
    </template>
    <radiobox v-for="unit of measureUnits" v-model="material.item.measureUnit" :value="unit">
        <span>{{locale.measureUnits[unit]}} ({{unit}})</span>
    </radiobox>
</groupbox>
<inputbox type="number" v-model="material.item.pricePerUnit" :invalid="badFields.pricePerUnit">
    {{locale.resource.pricePerUnit}}, &#8372;</inputbox>
<inputbox type="number" v-model="material.inStock" :invalid="badFields.inStock">
    {{locale.resource.inStock}}, {{locale.measureUnits[material.item.measureUnit]}}</inputbox>
<inputbox type="number" v-model="material.normalStock" :invalid="badFields.normalStock">
    {{locale.resource.normalStock}}, {{locale.measureUnits[material.item.measureUnit]}}</inputbox>
<inputbox type="number" v-model="material.refillPeriodDays" :invalid="badFields.refillPeriodDays">
    {{locale.resource.refillPeriodDays}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(material.item.createdAt)">
    {{locale.resource.createdAt}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(material.lastRefilledAt)">
    {{locale.resource.lastRefilledAt}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(material.lastCheckedAt)">
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
    material: Object,
    successMessage: String,
    errorMessage: String,
    badFields: {
        type: Object,
        default: { }
    }
})

const emit = defineEmits([
    "click-reset",
    "click-submit"
])

const measureUnits = [
    "unknown",
    "m",
    "m2",
    "m3",
    "L",
    "Kg",
    "g"
]

</script>
