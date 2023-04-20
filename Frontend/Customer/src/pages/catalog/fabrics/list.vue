<template>
<Bread :crumbs="[
    { href: '/', text: 'fwsh' },
    { href: '/catalog', text: locale.common.catalog }
    ]" :last="locale.fabric.plural" />
<Pagination v-if="data.items?.length"
    :page="props.page" :previous="data.previous" :next="data.next"
    :items="data.items" :view="FabricView"
    :bind="item => ({ fabric: item })"
    @click-previous="goToPrevious"
    @click-next="goToNext"
    @click-item="goToItem"
    class="width-container pad-05 margin-bottom-1"
    class-item="card pad-1 margin-bottom-1">
    <template #title>
        <h2 class="margin-bottom-1">
            {{locale.fabric.catalog}} &ndash; {{locale.common.page}} {{props.page}}
        </h2>
    </template>
</Pagination>
<div v-else class="width-container card pad-1 margin-bottom-1">
    <h2>Something went wrong.</h2>
    <p>{{data.message}}</p>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import Pagination from "@/layout/Pagination.vue"
import FabricView from "@/comp/mini/FabricView.vue"
import Bread from "@/layout/Bread.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number
})

const data = reactive({
    previous: null,
    next: null,
    items: [ ]
})

watch(() => props.page, getFabrics, { immediate: true })

function goToPrevious() {
    if (data.previous != null)
        router.push(`/catalog/fabrics/list?page=${data.previous}`)
}

function goToNext() {
    if (data.next != null)
        router.push(`/catalog/fabrics/list?page=${data.next}`)
}

function goToItem(item) {
    console.log("Should go to " + item.id)
}

async function getFabrics() {
    axios.get({
        url: "/catalog/fabrics/list",
        params: { page: props.page },
        cacheTTL: 600
    })
    .then(({ status, data: response }) => {
        data.items = response.items
        data.previous = response.previous
        data.next = response.next
    })
    .catch(error => {
        console.error(error)
    })
}

</script>
