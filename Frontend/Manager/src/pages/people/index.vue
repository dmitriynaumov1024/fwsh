<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb last>{{locale.person.plural}}</Crumb>
</Bread>
<div class="width-container pad-05 mar-b-1">
    <h2 class="mar-b-1">{{locale.person.plural}}</h2>
    <router-link to="/people/customers/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.customer.plural}}</h3>
    </router-link>
    <router-link to="/people/workers/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.worker.plural}}</h3>
    </router-link>
    <router-link to="/people/suppliers/list?page=0" class="card pad-1 mar-b-1">
        <h3 class="text-center">{{locale.supplier.plural}}</h3>
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
    if (! storage.tmp?.profile) router.replace("/login?next=/people")
}, 120))

</script>