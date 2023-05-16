<template>
<div class="flex-row flex-pad-1 mini-resource-view">
    <img v-if="mat.photoUrl && hasPhoto" :src="cdnResolve(mat.photoUrl)" 
        @error="()=> { hasPhoto = false }" class="fabric-card-thumbnail">
    <div v-else-if="mat.color" class="fabric-card-thumbnail fabric-card-overlay" :style="{ backgroundColor: mat.color.rgbCode }"></div>
    <UnknownImage v-else class="fabric-card-thumbnail" />
    <div class="flex-grow">
        <p><b>{{mat.name}}</b></p>
        <p>{{mat.pricePerUnit}} &#8372; / {{locale.measureUnits[mat.measureUnit]}}</p>
    </div>
</div>
</template>

<script setup>
import { ref, inject } from "vue"
import { cdnResolve } from "@common/utils"
import { UnknownImage } from "@common/comp/icons"

const props = defineProps({
    mat: Object
})

const hasPhoto = ref(true)

const locale = inject("locale")

const emit = defineEmits([
    "click-quantity",
    "click-details"
])

</script>
