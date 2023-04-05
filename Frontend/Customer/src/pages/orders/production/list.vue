<template>
<div class="width-container card pad-1 margin-bottom-1">
    <Pagination v-if="items?.length"
        :page="page" :previous="previous" :next="next"
        :items="items" :view="$options.components.ProductionOrderView"
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

<script>
import Pagination from "@/layout/Pagination.vue"
import ProductionOrderView from "@/comp/mini/ProductionOrderView.vue"

const props = {
    tab: String,
    page: Number
}

function data() {
    return {
        previous: null,
        next: null,
        items: [ ]
    }
}

function goToPrevious() {
    if (this.previous != null)
        this.$router.push(`/orders/production/${this.tab}?page=${this.previous}`)
}

function goToNext() {
    if (this.next != null)
        this.$router.push(`/orders/production/${this.tab}?page=${this.next}`)
}

function goToItem(item) {
    if (item?.id)
        this.$router.push(`/orders/production/view/${item.id}`)
}

function mounted() {
    this.$axios.get({
        url: `/customer/orders/production/${this.tab}`,
        params: { page: this.page }
    })
    .then(({ status, data: response }) => {
        this.items = response.items
        this.previous = response.previous
        this.next = response.next
    })
    .catch(error => {
        console.error(error)
    })
}

export default {
    props,
    data,
    mounted,
    methods: {
        goToPrevious,
        goToNext,
        goToItem
    },
    components: {
        Pagination,
        ProductionOrderView
    }
}
</script>