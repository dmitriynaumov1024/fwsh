<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/resources">{{locale.resource.plural}}</Crumb>
    <Crumb last>{{locale.fabricType.plural}}</Crumb>
</Bread>
<Fetch url="/resources/fabrictypes/list"
    :params="{ page: props.page, reverse: props.reverse }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-05">{{locale.fabricType.plural}}</h2>
            <div class="flex-stripe flex-pad-1 mar-b-1">
                <button class="button button-secondary accent-weak text-strong">
                    {{locale.common.page}} {{props.page}}
                </button>
                <router-link :to="`/resources/fabrictypes/list?page=0&reverse=${!props.reverse}`" 
                    class="button button-secondary accent-weak text-strong">
                    {{locale.common.id}} {{reverse ? '▼' : '▲'}}
                </router-link>
                <span class="flex-grow"></span>
                <router-link to="/resources/fabrictypes/create" class="button button-primary">
                    + {{locale.fabricType.single}}
                </router-link>
            </div>
        </template>
        <template v-slot:repeating="{ item }">
            <FabricTypeView :ftype="item" @click="()=> goToItem(item)" class="card-card pad-1 mar-b-1" />
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import qs from "qs"
import { useRouter } from "vue-router"
import { inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
import FabricTypeView from "@/comp/mini/FabricTypeView.vue"

const router = useRouter()
const locale = inject("locale")

const props = defineProps({
    page: Number,
    reverse: Boolean
})

function goToPage (page) {
    const query = { page: page, reverse: props.reverse }
    if (page != null) router.push(`/resources/fabrictypes/list?${qs.stringify(query)}`)
}

function goToItem (item) {
    router.push(`/resources/fabrictypes/edit/${item.id}`)
}

</script>
