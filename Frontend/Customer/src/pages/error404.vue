<template>
<div class="width-container card pad-1">
    <h2>{{locale.noRoute.title}}</h2>
    <p>{{locale.noRoute.description}} /{{ path }} .</p>
</div>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    path: String
})

</script>
