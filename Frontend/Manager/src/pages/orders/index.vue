<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.order.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.productionOrder.plural}}</h2>
    <router-link to="/orders/production/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.list}}</h3>
    </router-link>
    <router-link to="/orders/production/archive?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.archive}}</h3>
    </router-link>
</div>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.repairOrder.plural}}</h2>
    <router-link to="/orders/repair/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.list}}</h3>
    </router-link>
    <router-link to="/orders/repair/archive?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.archive}}</h3>
    </router-link>
</div>
</template>

<script setup>
import { inject } from "vue"
import { Bread, Crumb } from "@common/comp/layout"

const locale = inject("locale")
</script>
