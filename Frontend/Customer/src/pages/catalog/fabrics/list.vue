<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/catalog">{{locale.common.catalog}}</Crumb>
    <Crumb last>{{locale.fabric.plural}}</Crumb>
</Bread>
<Fetch url="/catalog/fabrics/list"
    :params="{ page: props.page }" :cacheTTL="600"
    :check-data="data => data.items?.length"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next" 
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-1">{{locale.fabric.catalog}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <FabricView :fabric="item" class="card-card pad-1 mar-b-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import qs from "qs"
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import FabricView from "@/comp/mini/FabricView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number,
    color: Number,
    fabrictype: Number,
    reverse: Boolean
})

function goToPage (page) {
    if (page != null && page != undefined) {
        let query = qs.stringify({ ...props, page: page })
        router.push(`/catalog/fabrics/list?${query}`)
    }
}

</script>
