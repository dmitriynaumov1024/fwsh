<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/people">{{locale.person.plural}}</Crumb>
    <Crumb last>{{locale.customer.plural}}</Crumb>
</Bread>
<Fetch url="/manager/customers/list"
    :params="{ page: props.page }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        @click-item="goToItem"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-1">{{locale.customer.plural}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <PersonView :person="item" @click="goToItem(item)" class="card-card pad-1 mar-b-1" />
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
import PersonView from "@/comp/mini/PersonView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/people/customers/list?page=${page}`)
}

function goToItem (item) {
    console.log("Should go to "+item.id)
    // if (item.id) 
    //     router.push(`/people/customers/view/${item.id}`)
}

</script>
