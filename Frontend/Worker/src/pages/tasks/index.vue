<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb last>{{locale.task.plural}}</crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.productionTask.plural}}</h2>
    <router-link to="/tasks/production/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.list}}</h3>
    </router-link>
    <router-link to="/tasks/production/archive?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.archive}}</h3>
    </router-link>
</div>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.repairTask.plural}}</h2>
    <router-link to="/tasks/repair/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.list}}</h3>
    </router-link>
    <router-link to="/tasks/repair/archive?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.common.archive}}</h3>
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
    if (! storage.tmp?.profile) router.replace("/login?next=/tasks")
}, 120))

</script>
