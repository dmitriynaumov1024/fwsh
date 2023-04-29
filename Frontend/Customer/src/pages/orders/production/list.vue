<template>
<Fetch :url="'/customer/orders/production/'+props.tab"
    :params="{ page: props.page }" :cacheTTL="2"
    class-error="width-container card pad-1 margin-bottom-1">
    <template v-slot:default="{ data }">
    <Pagination v-if="data.items?.length"
        :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 margin-bottom-1">
        <template v-slot:title>
            <h2 class="margin-bottom-1">
                {{locale.productionOrder.plural}} &ndash; {{locale.common[tab]}} 
                &ndash; {{locale.common.page}} {{props.page}}
            </h2>
        </template>
        <template v-slot:repeating="{ item }">
            <ProductionOrderView :order="item" @click="()=> goToItem(item)" class="card pad-1 margin-bottom-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Fetch } from "@common/comp/special"
import { Pagination } from "@common/comp/layout"
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
        router.push(`/catalog/fabrics/list?page=${page}`)
}

function goToItem (item) {
    router.push(`/orders/production/view/${item?.id}`)
}

</script>
