<template>
<h2 v-if="ftype.id" class="mar-b-1">{{locale.fabricType.single}} #{{ftype.id}}</h2>
<h2 v-else class="mar-b-1">{{locale.fabricType.create}}</h2>

<inputbox type="text" v-model="ftype.name" :invalid="!!badFields.name">
    {{locale.fabricType.name}}</inputbox>
<textbox v-model="ftype.description" :invalid="!!badFields.description">
    {{locale.fabricType.description}}</textbox>
<inputbox v-if="ftype.createdAt" disabled :value="locale.formatDateTime(ftype.createdAt)">
    {{locale.resource.createdAt}}</inputbox>

<div class="mar-b-2">
    <p v-if="errorMessage" class="text-error">{{errorMessage}}</p>
    <p v-if="successMessage">{{successMessage}}</p>
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
    ftype: Object,
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
