<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/catalog">{{locale.common.catalog}}</Crumb>
    <Crumb last>{{locale.fabric.plural}}</Crumb>
</Bread>
<Fetch url="/catalog/fabrics/list"
    :params="{ page: props.page }" :cacheTTL="600"
    class-error="width-container card pad-1 margin-bottom-1">
    <template v-slot:default="{ data }">
    <Pagination v-if="data.items?.length"
        :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next" 
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        class="width-container pad-05 margin-bottom-1">
        <template v-slot:title>
            <h2 class="margin-bottom-1">{{locale.fabric.catalog}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <FabricView :fabric="item" class="card pad-1 margin-bottom-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import FabricView from "@/comp/mini/FabricView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/catalog/fabrics/list?page=${page}`)
}

</script>
