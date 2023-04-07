<template>
<div class="flex-stripe">
    <svg style="display: inline-block; width: 1rem; border: var(--border); margin-right: 0.5rem;" viewBox="0 0 16 16" >
        <path v-if="checked" stroke="var(--color-accent)" stroke-width="3" fill="none" d="M 3 7 L 8 12 L 15 3" />
    </svg>
    <slot></slot>
    <span class="flex-grow"></span>
</div>
</template>

<script setup>
const props = defineProps({
    checked: Boolean
})
</script>