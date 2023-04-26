<template>
<div class="">
    <p><b>{{mat.item.name}}</b>&ensp;<span class="text-gray">#{{mat.id}}</span></p>
    <p>{{locale.resource.externalId}}: {{mat.externalId}}</p>
    <p>{{locale.resource.price}}: {{mat.item.pricePerUnit}} &#8372; / {{locale.measureUnits[mat.item.measureUnit]}}</p>
    <p>{{locale.resource.inStock}}: {{mat.inStock}} / {{mat.normalStock}} {{locale.measureUnits[mat.item.measureUnit]}}</p>
</div>
</template>

<script setup>
import { inject } from "vue"

const props = defineProps({
    mat: Object
})

const locale = inject("locale")

</script>
