<template>
<div class="flex-row flex-pad-1">
    <div class="flex-grow">
        <p class="mar-b-05"><b>{{ftype.name}}</b>&ensp;<span class="text-gray">#{{ftype.id}}</span></p>
        <p>{{ftype.description}}</p>
    </div>
</div>
</template>

<script setup>
const props = defineProps({
    ftype: Object
})    
</script>
