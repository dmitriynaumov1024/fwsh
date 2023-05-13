<template>
<div>
    <p>
        <b v-if="order.design?.displayName">{{order.design.displayName}}</b>
        <b v-else>{{locale.design.single}} #{{order.designId}}</b>
    </p>
    <p>{{locale.order.status}}: {{locale.status[order.status] ?? order.status}}</p>
    <p>{{locale.productionOrder.priceTotal}}: 
        {{order.pricePerOne}} &#8372; x {{order.quantity}} = <b>{{order.price || order.pricePerOne * order.quantity}} &#8372;</b></p>
</div>
</template>

<script setup>
import { inject } from "vue"

const props = defineProps({
    order: Object
})

const locale = inject("locale")

</script>
