<template>
<div class="flex-stripe">
    <svg class="fancy-checkbox-button" viewBox="0 0 16 16" :checked="thisIsChecked" @click="toggle">
        <path v-if="thisIsChecked" stroke="var(--color-accent)" 
            stroke-width="3" fill="none" d="M 3 7 L 8 12 L 18 2" />
    </svg>
    <div class="fancy-checkbox-label" @click="toggle"><slot></slot></div>
    <div class="flex-grow"></div>
</div>
</template>

<script setup>
import { computed } from "vue"

const props = defineProps({
    checked: Boolean,
    value: undefined,
    modelValue: [ Boolean, Array ]
})

const thisIsChecked = computed (
    () => props.checked == true 
        || props.modelValue == true 
        || props.modelValue instanceof Array 
        && props.modelValue.includes(props.value)
)

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    if (props.modelValue instanceof Array) {
        if (thisIsChecked.value) {
            emit("update:modelValue", props.modelValue.filter(a => a != props.value))
        }
        else {
            emit("update:modelValue", props.modelValue.concat(props.value))
        }
    }
    else {
        emit("update:modelValue", !props.modelValue)
    }
}

</script>
