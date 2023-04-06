<template>
<div class="pad-05">
    <p><b>{{design.displayName}}</b></p>
    <p>{{design.type}}</p>
    <p>{{design.price}} &#8372;</p>
</div>
</template>

<script setup>
const props = defineProps({
    design: {
        type: Object,
        default: { }
    }
})

</script>