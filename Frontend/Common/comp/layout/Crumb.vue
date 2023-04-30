<template>
<template v-if="to">
    <router-link :to="to" class="link text-thin"><slot /></router-link>&nbsp;&nbsp;/&nbsp;
</template>
<template v-else>
    <span class="link text-thin"><slot /></span>
</template>
</template>

<script setup>
const props = defineProps({
    to: String
})    
</script>
