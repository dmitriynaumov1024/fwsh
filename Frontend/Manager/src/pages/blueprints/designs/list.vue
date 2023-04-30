<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.common.blueprints}}</Crumb>
    <Crumb last>{{locale.design.plural}}</Crumb>
</Bread>
<Fetch url="/manager/designs/list"
    :params="{ page: props.page }" :cacheTTL="4"
    :check-data="data => data.items?.length"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 margin-bottom-1">
        <template v-slot:title>
            <h2 class="margin-bottom-1">{{locale.design.plural}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <DesignView :design="item" @click="()=> goToItem(item)" class="card-card pad-1 margin-bottom-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import DesignView from "@/comp/mini/DesignView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number
})

function goToPage() {
    if (page != null && page != undefined)
        router.push(`/blueprints/designs/list?page=${page}`)
}

function goToItem(item) {
    if (item.id) 
        router.push(`/blueprints/designs/view/${item.id}`)
}

</script>
