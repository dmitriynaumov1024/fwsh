<template>
<div>
    <p><b>{{locale.repairOrder.single}} #{{order.id}}</b></p>
    <p>{{locale.order.status}}: {{locale.status[order.status] ?? order.status}}</p>
    <p>{{locale.repairOrder.price}}: <b>{{order.price}} &#8372;</b></p>
</div>
</template>

<script setup>
import { inject } from "vue"

const props = defineProps({
    order: Object
})

const locale = inject("locale")

</script>
