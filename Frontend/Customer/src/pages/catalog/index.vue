<template>
<Bread :crumbs="[{href: '/', text: 'fwsh'}]" 
    :last="locale.common.catalog" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.common.catalog}}</h2>
    <router-link to="/catalog/designs/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.design.plural}}
    </router-link>
    <router-link to="/catalog/fabrics/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.fabric.plural}}
    </router-link>
    <router-link to="#" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        Something else
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
