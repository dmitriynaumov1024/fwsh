<template>
<ProductionOrderView v-if="data.order?.id" :order="data.order" />
<div v-else class="width-container text-center pad-1">
    <p>Loading, please wait...</p>
</div>
</template>

<script setup>
import { reactive, onMounted } from "vue" 
import ProductionOrderView from "@/comp/ProductionOrderView.vue"

const props = defineProps({
    id: Number
})

const data = reactive({
    order: null
})

onMounted(() => {
    axios.get({
        url: `/customer/orders/production/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (response.id) {
            data.order = response
        }
    })
    .catch(error => {
        console.log("Something went wrong")
    })
})

</script>