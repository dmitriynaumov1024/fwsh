<template>
<h2 v-if="mat.id" class="mar-b-1">{{locale.fabric.single}} <span class="text-thin text-gray">#{{mat.id}}</span></h2>
<h2 v-else class="mar-b-1">{{locale.fabric.create}}</h2>

<inputbox type="text" v-model="mat.name" :invalid="badFields.name">
    {{locale.resource.name}}</inputbox>
<inputbox type="text" v-model="mat.externalId" :invalid="badFields.externalId">
    {{locale.resource.externalId}}</inputbox>
<textbox v-model="mat.description" :invalid="badFields.description">
    {{locale.resource.description}}</textbox>
<inputbox disabled :value="locale.measureUnits[mat.measureUnit]">
    {{locale.resource.measureUnit}}</inputbox>
<inputbox type="number" v-model="mat.precision" :invalid="badFields.precision">
    {{locale.resource.precision}}</inputbox>
<inputbox type="number" v-model="mat.pricePerUnit" :invalid="badFields.pricePerUnit">
    {{locale.resource.pricePerUnit}}, &#8372;</inputbox>
<inputbox type="number" v-model="mat.inStock" :invalid="badFields.inStock">
    {{locale.resource.inStock}}<span v-if="mat.measureUnit">, {{locale.measureUnits[mat.measureUnit]}}</span></inputbox>
<inputbox type="number" v-model="mat.normalStock" :invalid="badFields.normalStock">
    {{locale.resource.normalStock}}<span v-if="mat.measureUnit">, {{locale.measureUnits[mat.measureUnit]}}</span></inputbox>
<inputbox type="number" v-model="mat.refillPeriodDays" :invalid="badFields.refillPeriodDays">
    {{locale.resource.refillPeriodDays}}</inputbox>
<groupbox clickable @click="()=> emit('click-supplier')">
    <template v-slot:header>
        {{locale.supplier.single}}
    </template>
    <template v-if="mat.supplier">
        <p>{{mat.supplier.surname}} {{mat.supplier.name}}</p>
        <p>{{mat.supplier.orgName}}, {{mat.supplier.phone}}</p>
    </template>
    <template v-else>
        <p>{{locale.common.notSelected}}</p>
    </template>
</groupbox>
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
import { MeasureUnits } from "@common"
import { ref, inject } from "vue"
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
    "click-supplier",
    "click-reset",
    "click-submit"
])

</script>
