<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb last>{{locale.myOrder.plural}}</crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="mar-b-1">{{locale.myOrder.plural}}</h2>
    <div class="card pad-1 mar-b-1">
        <div class="flex-stripe">
            <h3 class="flex-grow">{{locale.productionOrder.plural}}</h3>
            <router-link to="/orders/production/list?page=0" class="button button-primary">
                {{locale.action.details}}
            </router-link>
        </div>
    </div>
    <div class="card pad-1 mar-b-1">
        <div class="flex-stripe">
            <h3 class="flex-grow">{{locale.repairOrder.plural}}</h3>
            <router-link to="/orders/repair/list?page=0" class="button button-primary">
                {{locale.action.details}}
            </router-link>
        </div>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"
import { Bread, Crumb } from "@common/comp/layout"

const locale = inject("locale")
</script>
