<template>
<h2 v-if="color.id" class="mar-b-1">{{locale.color.single}} #{{color.id}}</h2>
<h2 v-else class="mar-b-1">{{locale.color.create}}</h2>
<div :style="{ height: '3rem', backgroundColor: color.rgbCode, border: 'var(--border)' }"></div>
<div v-if="color.id" class="fancy-input">
    <label>{{locale.common.id}}</label>
    <input type="text" :value="color.id" disabled />
</div>
<div class="fancy-input" :invalid="!!badFields.name">
    <label for="input-color-name">{{locale.color.name}}</label>
    <input type="text" id="input-color-name" v-model="color.name" />
</div>
<div class="fancy-input" :invalid="!!badFields.rgbCode">
    <label for="input-color-rgbcode">{{locale.color.rgbCode}}</label>
    <input type="text" id="input-color-rgbcode" v-model="color.rgbCode" class="text-mono" />
</div>
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

const locale = inject("locale")

const props = defineProps({
    color: Object,
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
