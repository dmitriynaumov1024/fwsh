<template>
<Bread> 
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.blueprint.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.blueprint.plural}}</h2>
    <router-link to="/blueprints/designs/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.design.plural}}</h3>
    </router-link>
    <router-link to="/blueprints/designs/create" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.design.create}}</h3>
    </router-link>
    <router-link to="/" class="card pad-1 mar-b-1">
        <h3 class="text-center">...</h3>
    </router-link>
</div>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject, onMounted } from "vue"
import { Bread, Crumb } from "@common/comp/layout"

const locale = inject("locale")
const storage = inject("storage")

const router = useRouter()

onMounted(() => setTimeout(()=> {
    if (! storage.tmp?.profile) router.replace("/login?next=/blueprints")
}, 120))

</script>
