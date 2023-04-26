<template>
<div class="flex-row flex-pad-1">
    <div class="color-card-thumbnail" :style="{ backgroundColor: color.rgbCode }"></div>
    <div class="flex-grow">
        <p><b>{{color.name}}</b></p>
        <p class="color-gray">{{color.rgbCode}}</p>
    </div>
</div>
</template>

<script setup>
const props = defineProps({
    color: Object
})    
</script>
