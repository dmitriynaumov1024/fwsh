<template>
<div class="width-container pad-1">
    <h2 class="mar-b-1">{{locale.index.title}}</h2>
    <router-link to="/resources" class="card pad-1 mar-b-1">
        <h3>{{locale.resource.plural}} &gt;</h3>
    </router-link>
    <router-link to="/tasks" class="card pad-1 mar-b-1">
        <h3>{{locale.task.plural}} &gt;</h3>
    </router-link>
    <router-link to="/" class="card pad-1 mar-b-1">
        <h3 class="text-center">...</h3>
    </router-link>
</div>
</template>

<script setup>
import { ref, inject } from "vue"

const locale = inject("locale")

</script>
