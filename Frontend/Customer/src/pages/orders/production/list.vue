<template>
<div class="width-container card pad-1 margin-bottom-1">
    <Pagination v-if="data.items?.length"
        :page="props.page" :previous="data.previous" :next="data.next"
        :items="data.items" :view="ProductionOrderView"
        :bind="item => ({ order: item })"
        @click-previous="goToPrevious"
        @click-next="goToNext" 
        @click-item="goToItem">
        <template #title>
            <h2>My production order {{ tab }} &ndash; Page {{ page }}</h2>
        </template>
    </Pagination>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import Pagination from "@/layout/Pagination.vue"
import ProductionOrderView from "@/comp/mini/ProductionOrderView.vue"

const router = useRouter()

const props = defineProps({
    tab: String,
    page: Number
})

const data = reactive({
    previous: null,
    next: null,
    items: [ ]
})

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

function mounted() {
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
