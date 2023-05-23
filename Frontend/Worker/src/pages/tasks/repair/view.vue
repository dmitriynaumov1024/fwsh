<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/repair/${props.tab??'list'}?page=0`">{{locale.repairTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<RepairOrderView v-if="data.task?.order"
    :order="data.task.order" 
    class="width-container card pad-1 mar-b-1" />
<RepairTaskView v-if="data.task" 
    :task="data.task"
    :errorMessage="data.errorMessage"
    :successMessage="data.successMessage" 
    @click-set-status="setStatus"
    @click-submit-usage="submitUsage"
    @click-reset-usage="resetUsage" />
<div v-else class="width-container card pad-1">
    <h2 class="mar-b-1">{{locale.noData.title}}</h2>
    <p>{{locale.noData.description}}</p>
</div>
</template>

<script setup>
import { jsonObjectCopy } from "@common/utils"
import { useRouter } from "vue-router" 
import { reactive, inject, watch } from "vue" 
import { Bread, Crumb } from "@common/comp/layout"
import RepairTaskView from "@/comp/views/RepairTaskView.vue"
import RepairOrderView from "@/comp/mini/RepairOrderView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    id: Number
})

let taskTemplate = undefined

const data = reactive({
    task: undefined,
    errorMessage: undefined,
    successMessage: undefined
})

watch(()=> props.id, getTask, { immediate: true })

function getTask () {
    axios.get({
        url: `/worker/tasks/repair/view/${props.id}`,
        cacheTTL: 1
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            taskTemplate = response
            data.task = jsonObjectCopy(response)
        }
    })
}

function setStatus (status) {
    if (!status) return

    axios.post({
        url: `/worker/tasks/repair/set-status/${props.id}`,
        params: { status: status }
    })
    .then(({ data: response }) => {
        if (response.success) {
            data.task.status = status
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        console.log(error)
        data.errorMessage = locale.value.error.description
    })
}

function submitUsage () {
    axios.post({
        url: `/worker/tasks/repair/set-usage/${props.id}`,
        data: data.task.resources
    })
    .then(({ status, data: response }) => {
        if (response.success) { 
            data.successMessage = locale.value.changesSaved.description
            getTask()
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        console.log(error)
        data.errorMessage = locale.value.error.description
    })
}

function resetUsage () {
    data.task.resources = jsonObjectCopy(taskTemplate.resources)
}

</script>
