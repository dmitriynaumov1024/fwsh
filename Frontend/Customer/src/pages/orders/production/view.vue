<template>
<Bread v-if="data.order?.id" :crumbs="[
    { href: '/', text: 'fwsh' },
    { href: '/orders/production/list?page=0', text: locale.productionOrder.plural }
    ]" :last="locale.order.single+' #'+data.order?.id" />
<ProductionOrderView v-if="data.order?.id" :order="data.order" @click-design="goToDesign" />
<div v-else-if="data.error" class="width-container text-center pad-1">
    <p>{{locale.common.somethingWrong}}. {{locale.common.seeConsole}}</p>
</div>
<div v-else class="width-container text-center pad-1">
    <p>{{locale.common.loading}}</p>
</div>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { reactive, inject, watch } from "vue" 
import Bread from "@/layout/Bread.vue"
import ProductionOrderView from "@/comp/ProductionOrderView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

const data = reactive({
    order: null
})

watch(() => props.id, getOrder, { immediate: true })

function goToDesign () {
    setTimeout(() => {
        router.push(`/catalog/designs/view/${data.order.design.id}`)
    }, 200)
}

function getOrder () {
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
}

</script>
