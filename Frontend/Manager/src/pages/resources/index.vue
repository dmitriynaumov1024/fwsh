<template>
<Bread :crumbs="[{ href: '/', text: 'fwsh' }]" 
    :last="locale.resource.plural" />
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.resource.plural}}</h2>
    <router-link to="/resources/parts/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.part.plural}}</h3>
    </router-link>
    <router-link to="/resources/materials/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.material.plural}}</h3>
    </router-link>
    <router-link to="/resources/fabrics/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.fabric.plural}}</h3>
    </router-link>
</div>
<div class="width-container pad-05 margin-bottom-1">
    <h2 class="margin-bottom-1">{{locale.common.other}}</h2>
    <router-link to="/resources/colors/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.color.plural}}</h3>
    </router-link>
    <router-link to="/resources/fabrictypes/list?page=0" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.fabricType.plural}}</h3>
    </router-link>
</div>
</template>

<script setup>
import { inject } from "vue"
import Bread from "@/layout/Bread.vue"

const locale = inject("locale")
</script>
