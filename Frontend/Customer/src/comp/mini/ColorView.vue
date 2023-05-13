<template>
<div class="flex-row flex-pad-1">
    <div class="color-card-thumbnail" :style="{ backgroundColor: color.rgbCode }"></div>
    <div class="flex-grow">
        <p class="mar-b-05"><b>{{color.name}}</b>&ensp;<span class="text-gray">#{{color.id}}</span></p>
        <p>{{color.rgbCode}}</p>
    </div>
</div>
</template>

<script setup>
const props = defineProps({
    color: Object
})    
</script>
