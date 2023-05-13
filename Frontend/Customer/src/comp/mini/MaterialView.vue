<template>
<div class="flex-row flex-pad-1 mini-resource-view">
    <img v-if="false" :src="cdnResolve(mat.photoUrl)" class="fabric-card-thumbnail">
    <div v-else-if="mat.color" class="fabric-card-thumbnail" :style="{ backgroundColor: mat.color.rgbCode }"></div>
    <UnknownImage v-else class="fabric-card-thumbnail" />
    <div class="flex-grow">
        <p><b>{{mat.name}}</b></p>
        <p>{{mat.pricePerUnit}} &#8372; / {{locale.measureUnits[mat.measureUnit]}}</p>
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

const emit = defineEmits([
    "click-quantity",
    "click-details"
])

</script>
