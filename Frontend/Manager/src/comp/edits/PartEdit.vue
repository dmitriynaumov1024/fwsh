<template>
<h2 v-if="part.id" class="mar-b-1">{{locale.part.single}} #{{part.id}}</h2>
<h2 v-else class="mar-b-1">{{locale.part.create}}</h2>

<inputbox disabled :value="part.id" >
    {{locale.common.id}}</inputbox>
<inputbox type="text" v-model="part.item.name" :invalid="badFields.name">
    {{locale.resource.name}}</inputbox>
<inputbox type="text" v-model="part.externalId" :invalid="badFields.externalId">
    {{locale.resource.externalId}}</inputbox>
<textbox v-model="part.item.description" :invalid="badFields.description">
    {{locale.resource.description}}</textbox>
<inputbox type="number" v-model="part.item.pricePerUnit" :invalid="badFields.pricePerUnit">
    {{locale.resource.pricePerUnit}}, &#8372;</inputbox>
<inputbox type="number" v-model="part.inStock" :invalid="badFields.inStock">
    {{locale.resource.inStock}}</inputbox>
<inputbox type="number" v-model="part.normalStock" :invalid="badFields.normalStock">
    {{locale.resource.normalStock}}</inputbox>
<inputbox type="number" v-model="part.refillPeriodDays" :invalid="badFields.refillPeriodDays">
    {{locale.resource.refillPeriodDays}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(part.item.createdAt)">
    {{locale.resource.createdAt}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(part.lastRefilledAt)">
    {{locale.resource.lastRefilledAt}}</inputbox>
<inputbox disabled :value="locale.formatDateTime(part.lastCheckedAt)">
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
    "click-reset",
    "click-submit"
])

</script>
