<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/catalog">{{locale.common.catalog}}</Crumb>
    <Crumb last>{{locale.design.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-05">
    <h2 class="mar-b-1">{{locale.design.catalog}}</h2>
    <div class="flex-stripe flex-pad-1">
        <div class="button button-secondary accent-weak text-strong">{{locale.common.page}} {{props.page}}</div>
        <div class="flex-grow">
            <selectbox v-model="search.type" :items="furnitureTypes">
                <template v-slot:title>
                    {{locale.design.type}}: {{locale.furnitureTypes[search.type]}}
                </template>
                <template v-slot:repeating="{ item }">
                    <p class="pad-05">{{locale.furnitureTypes[item]}}</p>
                </template>
            </selectbox>
        </div>
    </div>
</div>
<Fetch url="/catalog/designs/list"
    :params="{ page: props.page, type: props.type }" :cacheTTL="600"
    :check-data="data => data.items?.length"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:repeating="{ item }">
            <DesignView :design="item" @click="()=>goToItem(item)" class="card-card pad-1 mar-b-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import qs from "qs"
import { FurnitureTypes } from "@common"
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import { Selectbox } from "@common/comp/ctrl"
import { Fetch } from "@common/comp/special"
import DesignView from "@/comp/mini/DesignView.vue" 

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number,
    type: String,
    reverse: Boolean
})

const furnitureTypes = Object.values(FurnitureTypes)

const search = reactive({ 
    type: props.type
})

watch(() => search.type, ()=> goToPage(props.page, search.type)) 

function goToPage (page, type) {
    let query = qs.stringify({ page: page, type: type ?? search.type, reverse: props.reverse })
    if (page != null && page != undefined)
        router.push(`/catalog/designs/list?${query}`)
}

function goToItem(item) {
    router.push(`/catalog/designs/view/${item.id}`)
}

</script>
