<template>
<div>
    <p><b>{{fabric.name}}</b></p>
    <p>{{fabric.pricePerUnit}} &#8372;</p>
</div>
</template>

<script setup>
const props = defineProps({
    fabric: {
        type: Object,
        default: { }
    }
})
</script>
