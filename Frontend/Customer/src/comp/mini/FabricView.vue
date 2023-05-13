<template>
<div class="flex-row flex-pad-1 mini-resource-view">
    <img v-if="hasPhoto" :src="cdnResolve(fabric.photoUrl)" @error="()=> { hasPhoto = false }" class="fabric-card-thumbnail">
    <div v-if="fabric.color" class="fabric-card-thumbnail" :style="{ position: 'absolute', zIndex: 1, opacity: 0.5, backgroundColor: fabric.color.rgbCode }"></div>
    <UnknownImage v-else class="fabric-card-thumbnail" />
    <div class="flex-grow">
        <p><b>{{fabric.name}}</b></p>
        <p>{{fabric.pricePerUnit}} &#8372; / {{locale.measureUnits[fabric.measureUnit]}}</p>
    </div>
</div>
</template>

<script setup>
import { ref, inject } from "vue"
import { cdnResolve } from "@common/utils"
import { UnknownImage } from "@common/comp/icons"

const props = defineProps({
    fabric: Object
})

const hasPhoto = ref(true)

const locale = inject("locale")

const emit = defineEmits([
    "click-quantity",
    "click-details"
])

</script>
