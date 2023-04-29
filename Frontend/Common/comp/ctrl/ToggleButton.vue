<template>
<span @click="toggle">
    <slot :checked="modelValue == true" :active="modelValue == true">
        <svg class="fancy-radiobox-button" viewBox="0 0 16 16" :checked="modelValue == true">
            <rect v-if="modelValue == true" x="3" y="3" width="10" height="10" 
                fill="var(--color-accent)" stroke="none" stroke-width="0"/>
        </svg>
    </slot>
</span>
</template>

<script setup>
const props = defineProps({
    modelValue: Boolean
})

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    emit("update:modelValue", !props.modelValue)
}
</script>
