<template>
<Bread :crumbs="[
    { href: '/', text: 'fwsh' },
    { href: '/resources', text: locale.resource.plural } 
    ]"
    :last="locale.material.plural" />
<Pagination v-if="data.items?.length"
    :page="props.page" :previous="data.previous" :next="data.next"
    :items="data.items" :view="MaterialView"
    :bind="item => ({ mat: item })"
    @click-previous="goToPrevious"
    @click-next="goToNext"
    @click-item="goToItem"
    class="width-container pad-05 margin-bottom-1"
    class-item="card pad-1 margin-bottom-1">
    <template #title>
        <h2 class="margin-bottom-1">
            {{locale.material.plural}} &ndash; {{locale.common.page}} {{props.page}}
        </h2>
    </template>
</Pagination>
<div v-else class="width-container card pad-1">
    <h2 class="margin-bottom-1">{{locale.noData.title}}</h2>
    <p>{{locale.noData.description}}</p>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import Bread from "@/layout/Bread.vue"
import Pagination from "@/layout/Pagination.vue"
import MaterialView from "@/comp/mini/MaterialView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    page: Number
})

const data = reactive({
    previous: null,
    next: null,
    items: [ ]
})

watch(() => props.page, getMaterials, { immediate: true })

function goToPrevious() {
    if (data.previous != null)
        router.push(`/resources/materials/list?page=${data.previous}`)
}

function goToNext() {
    if (data.next != null)
        router.push(`/resources/materials/list?page=${data.next}`)
}

function goToItem (item) {
    console.log("Should go to "+item.id)
}

function getMaterials() {
    axios.get({
        url: "/resources/materials/list",
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
