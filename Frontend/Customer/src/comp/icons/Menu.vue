<template>
<svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 50 50">
    <path v-if="active" d="M 13 14 L 37 38 M 13 38 L 37 14"  
        stroke="var(--color-fore-0)" stroke-width="3.5" />
    <path v-else d="M 13 15 L 37 15 M 13 26 L 37 26 M 13 37 L 37 37" 
        stroke="var(--color-fore-0)" stroke-width="3" />
</svg>
</template>

<script>
export default {
    props: {
        active: Boolean
    }
}
</script>