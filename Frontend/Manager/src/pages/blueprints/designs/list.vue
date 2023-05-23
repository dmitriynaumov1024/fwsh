<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <Crumb last>{{locale.design.plural}}</Crumb>
</Bread>
<h2 class="width-container pad-05">{{locale.design.plural}}</h2>
<Fetch url="/manager/designs/list"
    :params="{ page: props.page }" :cacheTTL="4"
    :check-data="data => data.items?.length"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <div class="flex-stripe flex-pad-1 mar-b-1">
                <button class="button button-secondary accent-weak text-strong">
                    {{locale.common.page}} {{props.page}}</button>
                <span class="flex-grow"></span>
                <router-link to="/blueprints/designs/create" 
                    class="button button-primary">
                    + {{locale.design.single}}</router-link>
            </div>
        </template>
        <template v-slot:repeating="{ item }">
            <DesignView :design="item" clickable 
                @click="()=> goToItem(item)" 
                class="card-card pad-1 mar-b-1" />
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
