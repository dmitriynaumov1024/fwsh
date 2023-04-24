<template>
<Bread :crumbs="[{href: '/', text: 'fwsh'}]" 
    :last="locale.common.catalog" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.common.catalog}}</h2>
    <router-link to="/catalog/designs/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.design.plural}}</h3>
    </router-link>
    <router-link to="/catalog/fabrics/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.fabric.plural}}</h3>
    </router-link>
    <router-link to="#" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">...</h3>
    </router-link>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject } from "vue"

import Bread from "@/layout/Bread.vue"

const router = useRouter()
const locale = inject("locale")

</script>
