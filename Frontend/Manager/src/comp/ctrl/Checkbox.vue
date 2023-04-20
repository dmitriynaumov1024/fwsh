<template>
<div class="flex-stripe" @click="toggle">
    <svg class="fancy-checkbox-button" viewBox="0 0 16 16" >
        <path v-if="checked || modelValue" stroke="var(--color-accent)" stroke-width="2" fill="none" d="M 3 7 L 8 12 L 15 3" />
    </svg>
    <slot></slot>
    <span class="flex-grow"></span>
</div>
</template>

<script setup>
const props = defineProps({
    checked: Boolean,
    modelValue: Boolean
})

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    emit("update:modelValue", !props.modelValue)
}

</script>