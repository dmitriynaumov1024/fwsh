<template>
<div class="flex-row flex-wrap flex-pad-1">
    <img class="thumbnail" :src="cdnResolve(order.photoUrls[0])">
    <div>
        <p class="text-125 mar-b-05"><b>{{locale.order.single}}</b> #{{order.id}}</p>
        <p>{{locale.customer.single}}: #{{order.customerId}}</p>
        <p>{{locale.status[order.status]}}</p>
        <p>{{order.payment}} &#8372; / {{order.price}} &#8372; </p>
    </div>
</div>
</template>

<script setup>
import { cdnResolve } from "@common/utils"
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    order: {
        type: Object,
        default: { }
    }
})

</script>