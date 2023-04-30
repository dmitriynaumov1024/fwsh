<template>
<div class="width-container border-bottom bread">
    <div class="pad-05" style="">
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
