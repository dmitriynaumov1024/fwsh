<template>
<div class="flex-stripe">
    <svg class="fancy-radiobox-button" viewBox="0 0 16 16" :checked="thisIsChecked" @click="toggle">
        <rect v-if="thisIsChecked" x="3" y="3" width="10" height="10" 
            fill="var(--color-accent)" stroke="none" stroke-width="0"/>
    </svg>
    <div class="fancy-radiobox-label" @click="toggle"><slot></slot></div>
    <div class="flex-grow"></div>
</div>
</template>

<script setup>
import { computed } from "vue"

const props = defineProps({
    checked: Boolean,
    value: undefined,
    modelValue: undefined
})

const thisIsChecked = computed (
    () => props.checked == true 
        || props.modelValue == true 
        || (props.modelValue == props.value) && (props.value !== undefined)
)

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    if (props.value !== undefined) 
        emit("update:modelValue", props.value)
    else 
        emit("update:modelValue", !props.modelValue)
}

</script>
