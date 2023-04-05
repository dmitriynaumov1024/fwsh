<template>
<ProductionOrderView v-if="order?.id" :order="order" />
<div v-else class="width-container text-center pad-1">
    <p>Loading, please wait...</p>
</div>
</template>

<script>
import ProductionOrderView from "@/comp/ProductionOrderView.vue"

const props = {
    id: Number
}

function data() {
    return {
        order: null
    }
}

function mounted() {
    this.$axios.get({
        url: `/customer/orders/production/view/${this.id}`
    })
    .then(({ status, data: response }) => {
        if (response.id) {
            this.order = response
        }
    })
    .catch(error => {
        console.log("Something went wrong")
    })
}

export default {
    props, 
    data,
    mounted,
    components: {
        ProductionOrderView
    }
}
</script>