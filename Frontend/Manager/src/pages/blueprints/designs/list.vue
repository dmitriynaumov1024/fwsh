<template>
<Bread :crumbs="[
    { href: '/', text: 'fwsh' },
    { href: '/blueprints', text: locale.blueprint.plural }
    ]" :last="locale.design.plural" />
<Pagination v-if="data.items?.length"
    :page="props.page" :previous="data.previous" :next="data.next"
    :items="data.items" :view="DesignView"
    :bind="item => ({ design: item })"
    @click-previous="goToPrevious"
    @click-next="goToNext"
    @click-item="goToItem"
    class="width-container pad-05 margin-bottom-1"
    class-item="card pad-1 margin-bottom-1">
    <template #title>
        <h2 class="margin-bottom-1">
            {{locale.design.plural}} &ndash; {{locale.common.page}} {{props.page}}
        </h2>
    </template>
</Pagination>
</template>

<script setup>
import { useRouter } from "vue-router"
import { ref, reactive, inject, watch } from "vue"
import Bread from "@/layout/Bread.vue"
import Pagination from "@/layout/Pagination.vue"
import DesignView from "@/comp/mini/DesignView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    page: Number
})

const data = reactive({})

watch(() => props.page, getDesigns, { immediate: true })

function goToPrevious() {
    if (data.previous != null)
        router.push(`/blueprints/designs/list?page=${data.previous}`)
}

function goToNext() {
    if (data.next != null)
        router.push(`/blueprints/designs/list?page=${data.next}`)
}

function goToItem(item) {
    if (item.id) 
        router.push(`/blueprints/designs/view/${item.id}`)
}

async function getDesigns() {
    axios.get({
        url: "/manager/designs/list",
        params: { page: props.page },
        cacheTTL: 10
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