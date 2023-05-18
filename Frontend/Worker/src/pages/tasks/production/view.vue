<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/production/${props.tab??'list'}?page=0`">{{locale.productionTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<Fetch :url="`/worker/tasks/production/view/${id}`" :cacheTTL="4"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data: task }">
        <ProductionTaskView :task="task" /> 
    </template>   
</Fetch>

</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, inject } from "vue" 
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import ProductionTaskView from "@/comp/views/ProductionTaskView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    id: Number
})

</script>
