<template>
<div class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">{{design.displayName}}</h2>
    <table class="kvtable stripes margin-bottom-2">
        <tr>
            <td>{{locale.common.id}}</td>
            <td>#{{design.id}}</td>
        </tr>
        <tr>
            <td>{{locale.design.displayName}}</td>
            <td>{{design.displayName}}</td>
        </tr>
        <tr>
            <td>{{locale.design.type}}</td>
            <td>{{locale.furnitureTypes[design.type]}} ({{design.type}})</td>
        </tr>
        <tr>
            <td>{{locale.design.isVisible}}</td>
            <td>{{locale.yesNo[design.isVisible]}}</td>
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
    <div class="fancy-group margin-bottom-1">
        <header>{{locale.photo.plural}}</header>
        <main>
            <ImageGallery :items="design.photoUrls">
                <template v-slot="{ active, item }">
                    <img :src="cdnResolve(item)" :class="{ 'visible': active }">
                </template>
            </ImageGallery>
        </main>
    </div>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-primary" @click="()=>emit('click-edit')">{{locale.action.edit}}</button>
        <span class="flex-grow"></span>
        <button class="button button-secondary accent-bad" @click="()=>emit('click-delete')">{{locale.action.delete}}</button>
    </div>
</div>
</template>

<script setup>
import { cdnResolve } from "@common/utils"
import { inject } from "vue"
import { ImageGallery } from "@common/comp/layout"

const locale = inject("locale")

const props = defineProps({
    design: Object
})

const emit = defineEmits([
    "click-edit",
    "click-delete"
])

</script>