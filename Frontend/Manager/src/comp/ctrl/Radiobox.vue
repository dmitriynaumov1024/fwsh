<template>
<div class="flex-stripe" @click="toggle">
    <svg class="fancy-checkbox-button" viewBox="0 0 16 16" :checked="thisIsChecked">
        <rect v-if="thisIsChecked" fill="var(--color-accent)" 
            stroke="none" stroke-width="0"
            x="3" y="3" width="10" height="10" />
    </svg>
    <slot></slot>
    <span class="flex-grow"></span>
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
        || props.modelValue == props.value
)

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    if (props.value != undefined) 
        emit("update:modelValue", props.value)
    else 
        emit("update:modelValue", !props.modelValue)
}

</script>