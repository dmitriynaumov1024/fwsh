<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/profile">{{locale.profile.myProfile}}</Crumb>
    <Crumb last>{{locale.productionOrder.plural}}</Crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="margin-bottom-05">{{locale.productionOrder.plural}}</h2>
    <div class="flex-stripe flex-pad-1">
        <template v-for="nextTab of ['list', 'archive']">
            <button v-if="nextTab==props.tab" 
                class="button button-secondary accent-weak text-strong">{{locale.common[nextTab]}}</button>
            <router-link v-else :to="`/orders/production/${nextTab}?page=0`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
    </div>
</div>
<Fetch :url="`/customer/orders/production/${props.tab}`"
    :params="{ page: props.page }" :cacheTTL="2"
    :check-data="data => data.items?.length"
    class-error="width-container card pad-1 margin-bottom-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 margin-bottom-1">
        <template v-slot:title>
        </template>
        <template v-slot:repeating="{ item }">
            <ProductionOrderView :order="item" @click="()=> goToItem(item)" class="card-card pad-1 margin-bottom-1" />
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
import ProductionOrderView from "@/comp/mini/ProductionOrderView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    page: Number
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/orders/production/${props.tab}?page=${page}`)
}

function goToItem (item) {
    router.push(`/orders/production/view/${item?.id}`)
}

</script>
