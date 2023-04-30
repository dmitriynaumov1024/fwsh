<template>
<Fetch :url="`/manager/designs/view/${id}`" :cacheTTL="4" 
    @load="onLoad" no-default class-error="width-container card pad-1" />
<template v-if="design">
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.common.blueprints}}</Crumb>
    <Crumb to="/blueprints/designs/list?page=0">{{locale.design.plural}}</Crumb>
    <Crumb last>{{design.displayName}}</Crumb>
</Bread>
<DesignView :design="design"
    @click-edit="editDesign"
    @click-delete="deleteDesign" />
</template>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import DesignView from "@/comp/views/DesignView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    id: [ Number, String ]
})

const design = ref(null)

function onLoad (data) {
    design.value = data
}

function editDesign() {
    console.log("Should edit design")
}

function deleteDesign() {
    console.log("Should delete design")
}

</script>
