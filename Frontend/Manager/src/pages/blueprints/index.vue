<template>
<Bread :crumbs="[{href: '/', text: 'fwsh'}]" 
    :last="locale.blueprint.plural" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.blueprint.plural}}</h2>
    <router-link to="/blueprints/designs/list?page=0" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.design.plural}}
    </router-link>
    <router-link to="/blueprints/designs/create" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        {{locale.design.create}}
    </router-link>
    <router-link to="#" 
        class="button button-secondary button-block pad-1 margin-bottom-1 text-center">
        Something else
    </router-link>
</div>
</template>

<script setup>
import { inject } from "vue"
import Bread from "@/layout/Bread.vue"

const locale = inject("locale")
</script>
