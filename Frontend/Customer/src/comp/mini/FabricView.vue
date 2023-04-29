<template>
<div class="flex-row flex-pad-1">
    <div class="fabric-card-thumbnail" :style="{ backgroundColor: fabric.color.rgbCode }"></div>
    <div>
        <p><b>{{fabric.name}}</b></p>
        <p>{{fabric.pricePerUnit}} &#8372;</p>
    </div>
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
