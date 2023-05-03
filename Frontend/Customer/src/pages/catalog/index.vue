<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.common.catalog}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.common.catalog}}</h2>
    <router-link to="/catalog/designs/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.design.plural}}</h3>
    </router-link>
    <router-link to="/catalog/fabrics/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.fabric.plural}}</h3>
    </router-link>
    <router-link to="#" class="card pad-1 mar-b-1">
        <h3 class="text-center">...</h3>
    </router-link>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject } from "vue"

import { Bread, Crumb } from "@common/comp/layout"

const router = useRouter()
const locale = inject("locale")

</script>
