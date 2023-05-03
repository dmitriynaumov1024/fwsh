<template>
<div class="flex-row flex-wrap flex-pad-1">
    <img class="thumbnail" :src="cdnResolve(design.photoUrls[0])">
    <div>
        <p class="text-125 mar-b-05"><b>{{design.displayName}}</b></p>
        <p>{{locale.furnitureTypes[design.type]}} ({{design.type}})</p>
        <p>{{design.price}} &#8372;</p>
    </div>
</div>
</template>

<script setup>
import { cdnResolve } from "@common/utils"
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    design: {
        type: Object,
        default: { }
    }
})

</script>