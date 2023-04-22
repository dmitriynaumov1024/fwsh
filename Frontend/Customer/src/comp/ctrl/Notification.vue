<template>
<svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 50 50">
    <path v-if="active" d="M 13 14 L 37 38 M 13 38 L 37 14"  
        stroke="var(--color-fore-1)" stroke-width="3.5" />
    <template v-else>
    <path d="M18 12 L22 8 L28 8 L32 12 L37 33 L40 33 L40 40 L30 40 L27 44 L23 44 L20 40 L10 40 L10 33 L13 33 L18 12 Z" 
        stroke="none" fill="var(--color-fore-2)" />
    <circle v-if="unread" cx="41" cy="16" r="7" stroke="none" fill="var(--color-bad)" />
    </template>
</svg>
</template>

<script setup>
const props = defineProps({
    active: Boolean,
    unread: Boolean
})
</script>