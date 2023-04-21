<template>
<Bread :crumbs="[{href: '/', text: 'fwsh'}]" 
    :last="locale.person.plural" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.person.plural}}</h2>
    <router-link to="/people/customers/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.customer.plural}}
    </router-link>
    <router-link to="/people/workers/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.worker.plural}}
    </router-link>
    <router-link to="/people/suppliers/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.supplier.plural}}
    </router-link>
</div>
</template>

<script setup>
import { inject } from "vue"
import Bread from "@/layout/Bread.vue"

const locale = inject("locale")
</script>