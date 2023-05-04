<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/resources">{{locale.resource.plural}}</Crumb>
    <Crumb last>{{locale.color.plural}}</Crumb>
</Bread>
<Fetch url="/resources/colors/list"
    :params="{ page: props.page, reverse: props.reverse }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        @click-item="goToItem"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <!-- <h2 class="mar-b-05">{{locale.color.plural}} &ndash; {{locale.common.page}} {{props.page}}</h2> -->
            <h2 class="mar-b-05">{{locale.color.plural}}</h2>
            <div class="flex-stripe flex-pad-1 mar-b-1">
                <button class="button button-secondary accent-weak text-strong">
                    {{locale.common.page}} {{props.page}}
                </button>
                <router-link :to="`/resources/colors/list?page=0&reverse=${!props.reverse}`" 
                    class="button button-secondary accent-weak text-strong">
                    {{locale.common.id}} {{reverse ? '▼' : '▲'}}
                </router-link>
                <span class="flex-grow"></span>
                <router-link to="/resources/colors/create" class="button button-primary">
                    + {{locale.color.single}}
                </router-link>
            </div>
        </template>
        <template v-slot:repeating="{ item }">
            <ColorView :color="item" clickable @click="goToItem(item)" class="card-card pad-1 mar-b-1" />
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
import ColorView from "@/comp/mini/ColorView.vue"

const router = useRouter()
const locale = inject("locale")

const props = defineProps({
    page: Number,
    reverse: Boolean
})

function goToPage(page) {
    const query = { page: page, reverse: props.reverse }
    if (page != null) router.push(`/resources/colors/list?${qs.stringify(query)}`)
}

function goToItem(item) {
    if (item.id) router.push(`/resources/colors/edit/${item.id}`)
}

function goToCreate() {
    router.push(`/resources/colors/create`)
}

</script>
