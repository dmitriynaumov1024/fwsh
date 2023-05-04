<template>
<div class="flex-row flex-pad-1">
    <img v-if="mat.item.photoUrl" :src="cdnResolve(mat.item.photoUrl)" class="fabric-card-thumbnail">
    <div v-else-if="mat.item.color" class="fabric-card-thumbnail" :style="{ backgroundColor: mat.item.color.rgbCode }"></div>
    <UnknownImage v-else class="fabric-card-thumbnail" />
    <div>
        <p><b>{{mat.item.name}}</b>&ensp;<span class="text-gray">#{{mat.id}}</span></p>
        <p>{{locale.resource.externalId}}: {{mat.externalId}}</p>
        <p>{{locale.resource.price}}: {{mat.item.pricePerUnit}} &#8372; / {{locale.measureUnits[mat.item.measureUnit]}}</p>
        <p>{{locale.resource.inStock}}: {{mat.inStock}} / {{mat.normalStock}} {{locale.measureUnits[mat.item.measureUnit]}}</p>
    </div>
</div>
</template>

<script setup>
import { inject } from "vue"
import { cdnResolve } from "@common/utils"
import { UnknownImage } from "@common/comp/icons"

const props = defineProps({
    mat: Object
})

const locale = inject("locale")

</script>
