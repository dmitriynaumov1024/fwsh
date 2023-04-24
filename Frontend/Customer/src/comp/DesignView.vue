<template>
<div class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">{{design.displayName}}</h2>
    <ImageGallery class="margin-bottom-2">
        <img v-for="url of design.photoUrls" :src="resolvePhotoUrl(url)">
    </ImageGallery>
    <table class="kvtable stripes margin-bottom-2">
        <tr>
            <td>{{locale.design.type}}</td>
            <td>{{locale.furnitureTypes[design.type]}} [ {{design.type}} ]</td>
        </tr>
        <tr>
            <td>{{locale.design.isTransformable}}</td>
            <td>{{locale.yesNo[design.isTransformable]}}</td>
        </tr>
        <tr>
            <td>{{locale.design.dimCompact}}</td>
            <td>{{design.dimCompact.width}} x {{design.dimCompact.length}} x {{design.dimCompact.height}} cm</td>
        </tr>
        <tr v-if="design.isTransformable">
            <td>{{locale.design.dimExpanded}}</td>
            <td>{{design.dimExpanded.width}} x {{design.dimExpanded.length}} x {{design.dimExpanded.height}} cm</td>
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
        <button class="button button-primary button-block pad-1" @click="()=>emit('click-edit')">
            {{locale.action.makeOrder}}
        </button>
    </div>
</div>
</template>

<script setup>
import { inject } from "vue"
import ImageGallery from "@/comp/ctrl/ImageGallery.vue"

const locale = inject("locale")

const props = defineProps({
    design: Object
})

const emit = defineEmits([
    "click-edit",
    "click-delete"
])

function resolvePhotoUrl(url) {
    return import.meta.env.VITE_CDN_BASEURL + "/files/" + url
}

</script>
