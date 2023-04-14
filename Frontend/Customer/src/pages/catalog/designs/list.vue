<template>
<Bread :crumbs="[
    { href: '/', text: 'fwsh' },
    { href: '/catalog', text: locale.common.catalog }
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
            {{locale.design.catalog}} &ndash; {{locale.common.page}} {{props.page}}
        </h2>
    </template>
</Pagination>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import Pagination from "@/layout/Pagination.vue"
import DesignView from "@/comp/mini/DesignView.vue"
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

watch(() => props.page, getDesigns, { immediate: true })

function goToPrevious() {
    if (data.previous != null)
        router.push(`/catalog/designs/list?page=${data.previous}`)
}

function goToNext() {
    if (data.next != null)
        router.push(`/catalog/designs/list?page=${data.next}`)
}

function goToItem(item) {
    console.log("Should go to " + item.id)
}

async function getDesigns() {
    axios.get({
        url: "/catalog/designs/list",
        params: { page: props.page }
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
