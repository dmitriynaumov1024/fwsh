<template>
<div class="flex-row flex-pad-1 mini-resource-view">
    <img v-if="mat.photoUrl" :src="cdnResolve(mat.photoUrl)" class="fabric-card-thumbnail">
    <div v-else-if="mat.color" class="fabric-card-thumbnail" :style="{ backgroundColor: mat.color.rgbCode }"></div>
    <UnknownImage v-else class="fabric-card-thumbnail" />
    <table class="kntable stripes flex-grow">
        <tr>
            <td clickable @click="()=> emit('click-details')">
                <b>{{mat.name}}</b>&ensp;<span class="text-gray">#{{mat.id}}</span></td>
        </tr>
        <tr>
            <td>{{locale.resource.externalId}}</td>
            <td>{{mat.externalId}}</td>
        </tr>
        <tr>
            <td>{{locale.resource.pricePerUnit}}</td>
            <td>{{mat.pricePerUnit}} &#8372; / {{locale.measureUnits[mat.measureUnit]}}</td>
        </tr>
        <tr :class="{ 'text-bad': mat.needsRefill, 'text-warn': mat.isTimeToRefill }">
            <td>{{locale.resource.inStock}}</td>
            <td><p>{{mat.inStock}} / {{mat.normalStock}} {{locale.measureUnits[mat.measureUnit]}}</p>
                <button class="button button-inline" @click="()=> emit('click-quantity')">
                    <pencil-icon class="inline icon-1" /> {{locale.action.update}}
                </button>
            </td>
        </tr>
    </table>
</div>
</template>

<script setup>
import { inject } from "vue"
import { cdnResolve } from "@common/utils"
import { UnknownImage, PencilIcon } from "@common/comp/icons"

const props = defineProps({
    mat: Object
})

const locale = inject("locale")

const emit = defineEmits([
    "click-quantity",
    "click-details"
])

</script>
