<template>
<div class="width-container border-bottom bread">
    <div class="flex-stripe flex-wrap pad-05">
        <slot></slot>
        <span class="flex-grow"></span>
    </div>
</div>
</template>

<script setup>
const props = defineProps({
    crumbs: Array,
    last: String
})
</script>
