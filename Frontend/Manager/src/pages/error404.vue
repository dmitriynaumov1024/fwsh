<template>
<div class="width-container card pad-1">
    <h2>Error 404: Not found</h2>
    <p>This is most likely because you tried to navigate to non-existent page /{{ path }} .</p>
</div>
</template>

<script setup>
const props = defineProps({
    path: String
})

</script>
