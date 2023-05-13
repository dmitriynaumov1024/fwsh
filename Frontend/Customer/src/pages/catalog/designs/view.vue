<template>
<Fetch :url="`/catalog/designs/view/${id}`" :cacheTTL="600" @load="onLoad" no-default
    class-error="width-container card pad-1 mar-b-1" />
<template v-if="design">
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/catalog">{{locale.common.catalog}}</Crumb>
    <Crumb to="/catalog/designs/list?page=0">{{locale.design.plural}}</Crumb>
    <Crumb last>{{design.displayName}}</Crumb>
</Bread>
<DesignView :design="design" @click-order="makeOrder" />
</template>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, reactive, inject, watch } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import DesignView from "@/comp/views/DesignView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

const design = ref(null)

function onLoad (data) {
    design.value = data
}

function makeOrder (design) {
    router.push(`/orders/production/create?design=${design.nameKey}`)
}

</script>
