<template>
<table class="kntable stripes">
    <tr>
        <td><router-link class="link" :to="`/resources/${order.item.type}s/edit/${order.item.id}`">
            <b>{{order.item.name}}</b></router-link></td> 
        <td>{{order.externalId ?? order.item.externalId}}</td>
    </tr>
    <tr>
        <td>{{locale.supplier.single}}</td>
        <td><p>{{order.supplier.surname}} {{order.supplier.name}} {{order.supplier.patronym}}</p>
            <p>{{order.supplier.orgName}}</p></td>
    </tr>
    <tr>
        <td>{{locale.resource.quantity}}</td>
        <td>{{order.actualQuantity}} / {{order.expectQuantity}} 
        <template v-if="order.item.measureUnit">{{locale.measureUnits[order.item.measureUnit]}}</template></td>
    </tr>
    <tr>
        <td>{{locale.supplyOrder.expectPrice}}</td>
        <td>{{order.expectPricePerUnit}} &#8372;
        <template v-if="order.item.measureUnit">/ {{locale.measureUnits[order.item.measureUnit]}}</template></td>
    </tr>
    <tr v-if="order.actualPrice != 0">
        <td>{{locale.supplyOrder.actualPrice}}</td>
        <td>{{order.expectPricePerUnit}} &#8372;
        <template v-if="order.item.measureUnit">/ {{locale.measureUnits[order.item.measureUnit]}}</template></td>
    </tr>
    <tr>
        <td>{{locale.order.status}}</td>
        <td>{{locale.status[order.status]}}</td>
    </tr>
</table>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    order: Object
})

</script>
