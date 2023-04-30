<template>
<Bread> 
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.blueprint.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.blueprint.plural}}</h2>
    <router-link to="/blueprints/designs/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.design.plural}}</h3>
    </router-link>
    <router-link to="/blueprints/designs/create" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.design.create}}</h3>
    </router-link>
    <router-link to="/" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">...</h3>
    </router-link>
</div>
</template>

<script setup>
import { inject } from "vue"
import { Bread, Crumb } from "@common/comp/layout"

const locale = inject("locale")
</script>
