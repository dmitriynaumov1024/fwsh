<template>
<Bread :crumbs="[{href: '/', text: 'fwsh'}]" 
    last="Catalog" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">Catalog</h2>
    <router-link to="/catalog/designs/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        Furniture
    </router-link>
    <router-link to="/catalog/fabrics/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        Fabrics
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
