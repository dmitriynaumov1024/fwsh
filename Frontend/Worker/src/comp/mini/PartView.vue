<template>
<div class="">
    <p><b>{{part.item.name}}</b>&ensp;<span class="text-gray">#{{part.id}}</span></p>
    <p>{{locale.resource.externalId}}: {{part.externalId}}</p>
    <p>{{locale.resource.pricePerUnit}}: {{part.item.pricePerUnit}} &#8372;</p>
    <p>{{locale.resource.inStock}}: {{part.inStock}} / {{part.normalStock}}</p>
</div>    
</template>

<script setup>
import { inject } from "vue"

const props = defineProps({
    part: Object
})

const locale = inject("locale")

</script>
