<template>
<div class="width-container pad-1">
    <h2 class="margin-bottom-2">{{locale.index.title}}</h2>
    <router-link to="/people" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.person.plural}}</h3>
    </router-link>
    <router-link to="/resources" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.resource.plural}}</h3>
    </router-link>
    <router-link to="/blueprints" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.blueprint.plural}}</h3>
    </router-link>
    <router-link to="/orders" class="card pad-1 margin-bottom-1">
        <h3 class="text-center">{{locale.order.plural}}</h3>
    </router-link>
</div>
</template>

<script setup>
import { ref, inject } from "vue"

const locale = inject("locale")

</script>
