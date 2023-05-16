<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/repair/${props.tab??'list'}?page=0`">{{locale.repairTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<Fetch :url="`/manager/tasks/repair/view/${id}`" :cacheTTL="4"
    class-error="width-container card pad-1 mar-b-1">        
</Fetch>

</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    id: Number
})

</script>
