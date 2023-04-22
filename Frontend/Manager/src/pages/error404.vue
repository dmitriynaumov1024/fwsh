<template>
<div class="width-container card pad-1">
    <h2 class="margin-bottom-1">{{locale.noRoute.title}}</h2>
    <p>{{locale.noRoute.description}} /{{ path }} .</p>
</div>
</template>

<script setup>
import { inject } from "vue"

const props = defineProps({
    path: String
})

const locale = inject("locale")

</script>
