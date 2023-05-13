<template>
<div class="width-container card pad-1 mar-b-2">
    <h2 class="mar-b-1">{{design.displayName}}</h2>
    <ImageGallery :items="design.photoUrls" class="mar-b-2">
        <template v-slot="{ active, item }">
            <img :src="cdnResolve(item)" :class="{ 'visible': active }">
        </template>
    </ImageGallery>
    <table class="kvtable stripes mar-b-2">
        <tr>
            <td>{{locale.design.type}}</td>
            <td>{{locale.furnitureTypes[design.type]}} ({{design.type}})</td>
        </tr>
        <tr>
            <td>{{locale.design.isTransformable}}</td>
            <td>{{locale.yesNo[design.isTransformable]}}</td>
        </tr>
        <tr>
            <td>{{locale.design.dimCompact}}</td>
            <td>{{design.dimCompact.width}} x {{design.dimCompact.length}} x {{design.dimCompact.height}} 
                {{locale.measureUnits[design.dimCompact.measureUnit]}}</td>
        </tr>
        <tr v-if="design.isTransformable && design.dimExpanded">
            <td>{{locale.design.dimExpanded}}</td>
            <td>{{design.dimExpanded.width}} x {{design.dimExpanded.length}} x {{design.dimExpanded.height}} 
                {{locale.measureUnits[design.dimExpanded.measureUnit]}}</td>
        </tr>
        <tr>
            <td>{{locale.design.price}}</td>
            <td>{{design.price}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.design.description}}</td>
            <td>{{design.description}}</td>
        </tr>
    </table>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-primary button-block pad-1" @click="()=>emit('click-order', design)">
            {{locale.action.makeOrder}}
        </button>
    </div>
</div>
</template>

<script setup>
import { inject } from "vue"
import { cdnResolve } from "@common/utils" 
import { ImageGallery } from "@common/comp/layout"

const locale = inject("locale")

const props = defineProps({
    design: Object
})

const emit = defineEmits([
    "click-order"
])

</script>
