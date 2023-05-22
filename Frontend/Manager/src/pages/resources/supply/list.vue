<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb last>{{locale.supplyOrder.plural}}</crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="mar-b-1">{{locale.supplyOrder.plural}}</h2>
    <div class="flex-stripe flex-pad-1">
        <template v-for="nextTab of ['list', 'archive']">
            <button v-if="nextTab==props.tab" 
                class="button button-secondary accent-weak text-strong">{{locale.common[nextTab]}}</button>
            <router-link v-else :to="`/resources/supply/${nextTab}?page=0&resource=${props.resource}`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
        <router-link v-if="props.resource" :to="`/resources/supply/create?resource=${props.resource}`"
            class="button button-primary">+ {{locale.action.create}}</router-link>
    </div>
</div>
<Fetch :url="`/manager/orders/supply/${props.tab}`"
    :params="{ page: props.page, resource: props.resource }" :cacheTTL="10" 
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05">
        <template v-slot:repeating="{ item }">
            <SupplyOrderView :order="item" 
                :clickable="props.tab == 'list'"
                @click="goToItem(item)"
                class="card-card pad-1 mar-b-1"  />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import qs from "qs"
import { useRouter } from "vue-router"
import { reactive, inject } from "vue"
import { nestedObjectAssign } from "@common/utils"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Modal, Pagination } from "@common/comp/layout"
import SupplyOrderView from "@/comp/mini/SupplyOrderView.vue"

const locale = inject("locale")
const axios = inject("axios")
const router = useRouter()

const props = defineProps({
    tab: String,
    page: Number,
    resource: Number
})
    
function goToPage (page) {
    const query = qs.stringify({ page: page, resource: props.resource })
    router.push(`/resources/supply/${tab}?${query}`)
}

function goToItem (item) {
    if (props.tab == 'list' && item.id) 
        router.push(`/resources/supply/edit/${item.id}`)
}

</script>
