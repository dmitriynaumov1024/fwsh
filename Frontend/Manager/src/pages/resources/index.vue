<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.resource.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-05">{{locale.supplyOrder.plural}}</h2>
    <router-link to="/resources/supply/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.common.list}}</h3>
    </router-link>
    <router-link to="/resources/supply/archive?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.common.archive}}</h3>
    </router-link>
</div>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-05">{{locale.resource.plural}}</h2>
    <router-link to="/resources/parts/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.part.plural}}</h3>
    </router-link>
    <router-link to="/resources/materials/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.material.plural}}</h3>
    </router-link>
    <router-link to="/resources/fabrics/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.fabric.plural}}</h3>
    </router-link>
</div>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-05">{{locale.common.other}}</h2>
    <router-link to="/resources/colors/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.color.plural}}</h3>
    </router-link>
    <router-link to="/resources/fabrictypes/list?page=0" class="card-card pad-05 mar-b-05">
        <h3 class="text-center">{{locale.fabricType.plural}}</h3>
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
    if (! storage.tmp?.profile) router.replace("/login?next=/resources")
}, 120))

</script>
