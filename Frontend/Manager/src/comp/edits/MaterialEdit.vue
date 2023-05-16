<template>
<h2 v-if="mat.id" class="mar-b-1">{{locale.material.single}} #{{mat.id}}</h2>
<h2 v-else class="mar-b-1">{{locale.material.create}}</h2>

<inputbox disabled :value="mat.id" >
    {{locale.common.id}}</inputbox>
<inputbox type="text" v-model="mat.name" :invalid="badFields.name">
    {{locale.resource.name}}</inputbox>
<inputbox type="text" v-model="mat.externalId" :invalid="badFields.externalId">
    {{locale.resource.externalId}}</inputbox>
<textbox v-model="mat.description" :invalid="badFields.description">
    {{locale.resource.description}}</textbox>
<groupbox :invalid="badFields.measureUnit">
    <template v-slot:header>
        {{locale.resource.measureUnit}}
    </template>
    <radiobox v-for="unit of measureUnits" v-model="mat.measureUnit" :value="unit">
        <span>{{locale.measureUnits[unit]}} ({{unit}})</span>
    </radiobox>
</groupbox>
<inputbox type="number" v-model="mat.pricePerUnit" :invalid="badFields.pricePerUnit">
    {{locale.resource.pricePerUnit}}, &#8372;</inputbox>
<inputbox type="number" v-model="mat.inStock" :invalid="badFields.inStock">
    {{locale.resource.inStock}}, {{locale.measureUnits[mat.measureUnit]}}</inputbox>
<inputbox type="number" v-model="mat.normalStock" :invalid="badFields.normalStock">
    {{locale.resource.normalStock}}, {{locale.measureUnits[mat.measureUnit]}}</inputbox>
<inputbox type="number" v-model="mat.refillPeriodDays" :invalid="badFields.refillPeriodDays">
    {{locale.resource.refillPeriodDays}}</inputbox>
<inputbox v-if="mat.createdAt" disabled :value="locale.formatDateTime(mat.createdAt)">
    {{locale.resource.createdAt}}</inputbox>
<inputbox v-if="mat.lastRefilledAt" disabled :value="locale.formatDateTime(mat.lastRefilledAt)">
    {{locale.resource.lastRefilledAt}}</inputbox>
<inputbox v-if="mat.lastCheckedAt" disabled :value="locale.formatDateTime(mat.lastCheckedAt)">
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
    mat: Object,
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
