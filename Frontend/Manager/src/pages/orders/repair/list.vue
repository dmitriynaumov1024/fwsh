<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/orders">{{locale.order.plural}}</Crumb>
    <Crumb last>{{locale.repairOrder.plural}}</Crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="mar-b-05">{{locale.repairOrder.plural}}</h2>
    <div class="flex-stripe flex-pad-1">
        <template v-for="nextTab of ['list', 'archive']">
            <button v-if="nextTab==props.tab" 
                class="button button-secondary accent-weak text-strong">{{locale.common[nextTab]}}</button>
            <router-link v-else :to="`/orders/repair/${nextTab}?page=0`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
    </div>
</div>
<Fetch :url="`/manager/orders/repair/${props.tab}`"
    :params="{ page: props.page }" :cacheTTL="2"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
        </template>
        <template v-slot:repeating="{ item }">
            <RepairOrderView :order="item" clickable
                @click="()=> goToItem(item)" class="card-card pad-1 mar-b-1" />
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
import RepairOrderView from "@/comp/mini/RepairOrderView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    page: Number,
    order: Number
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/orders/repair/${props.tab??'list'}?page=${page}`)
}

function goToItem (item) {
    router.push(`/orders/repair/view/${item?.id}?tab=${props.tab}`)
}

</script>
