<template>
<Pagination v-if="data.items?.length"
    :page="props.page" :previous="data.previous" :next="data.next"
    :items="data.items" :view="ProductionOrderView"
    :bind="item => ({ order: item })"
    @click-previous="goToPrevious"
    @click-next="goToNext" 
    @click-item="goToItem"
    class="width-container pad-05 margin-bottom-1"
    class-item="card pad-1 margin-bottom-1">
    <template #title>
        <h2 class="margin-bottom-1">
            {{locale.productionOrder.plural}} &ndash; {{locale.common[tab]}} 
            &ndash; {{locale.common.page}} {{props.page}}
        </h2>
    </template>
</Pagination>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import Pagination from "@/layout/Pagination.vue"
import ProductionOrderView from "@/comp/mini/ProductionOrderView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    page: Number
})

const data = reactive({
    previous: null,
    next: null,
    items: [ ]
})

watch(() => props, getOrders, { immediate: true })

function goToPrevious() {
    if (data.previous != null)
        router.push(`/orders/production/${props.tab}?page=${data.previous}`)
}

function goToNext() {
    if (data.next != null)
        router.push(`/orders/production/${props.tab}?page=${data.next}`)
}

function goToItem(item) {
    if (item?.id)
        router.push(`/orders/production/view/${item.id}`)
}

function getOrders() {
    axios.get({
        url: `/customer/orders/production/${props.tab}`,
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
