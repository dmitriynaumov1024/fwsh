<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/resources">{{locale.resource.plural}}</Crumb>
    <Crumb last>{{locale.color.plural}}</Crumb>
</Bread>
<Fetch url="/resources/colors/list"
    :params="{ page: props.page }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=>goToPage(data.previous)"
        @click-next="()=>goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-1">{{locale.color.plural}} &ndash; {{locale.common.page}} {{props.page}}</h2>
        </template>
        <template v-slot:repeating="{ item }">
            <ColorView :color="item" class="card-card pad-1 mar-b-1" />
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
import ColorView from "@/comp/mini/ColorView.vue"

const router = useRouter()
const locale = inject("locale")

const props = defineProps({
    page: Number
})

function goToPage(page) {
    if (page != null) router.push(`/resources/colors/list?page=${page}`)
}

</script>
