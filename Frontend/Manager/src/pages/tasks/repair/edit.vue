<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/repair/list?page=0`">{{locale.repairTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<div class="width-container card pad-1">
    <RepairOrderView v-if="data.order" :order="data.order"
        clickable @click="goToOrder(data.order)" 
        class="mar-b-1" />
    <RepairTaskEdit v-if="data.task" 
        :task="data.task"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetTask"
        @click-submit="submitTask" 
        @click-add-resource="beginAddResource" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>
<Modal v-if="data.resources">
    <h3 class="mar-b-1">{{locale[data.resourceType].plural}}</h3>
    <Pagination :items="data.resources.items" :page="data.resources.page"
        :previous="data.resources.previous" :next="data.resources.next"
        @click-previous="()=> { data.resourcePage -= 1 }"
        @click-next="()=> { data.resourcePage += 1 }"
        class="mar-b-1">
        <template v-slot:repeating="{ item }">
        <radiobox v-model="data.resources.selected" :value="item" class="mar-b-05">{{item.name}}</radiobox>
        </template>
    </Pagination>
    <div class="flex-stripe">
        <button class="button button-inline accent-gray" 
            @click="endAddResource">{{locale.action.cancel}}</button>
        <span class="flex-grow"></span>
        <button v-if="data.resources.selected" class="button button-primary" 
            @click="endAddResource">{{locale.action.confirm}}</button>
    </div>
</Modal>
</template>

<script setup>
import { arrayToDict, jsonObjectCopy } from "@common/utils"
import { useRouter } from "vue-router"
import { reactive, watch, inject } from "vue"
import { Bread, Crumb, Modal, Pagination } from "@common/comp/layout"
import { Radiobox } from "@common/comp/ctrl"
import RepairOrderView from "@/comp/mini/RepairOrderView.vue"
import RepairTaskEdit from "@/comp/edits/RepairTaskEdit.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number,
    order: Number
})

let taskTemplate = { 
    description: "...",
    payment: 0, 
    orderId: props.order,
    resources: [ ]
}

const data = reactive({
    order: undefined,
    task: undefined,
    resourceType: undefined,
    resourcePage: undefined,
    resources: undefined,
    badFields: { },
    errorMessage: undefined,
    successMessage: undefined
})

watch(()=> props.id, getTask, { immediate: true })

function getTask() {
    data.errorMessage = undefined

    if (!props.id) {
        data.task = jsonObjectCopy(taskTemplate)
        getOrder()
        return
    }

    axios.get({
        url: `/manager/tasks/repair/view/${props.id}`,
        cacheTTL: 3
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            taskTemplate = response
            data.task = jsonObjectCopy(taskTemplate)
            if (response.order) data.order = response.order
            else getOrder()
        }
        else {
            data.task = undefined
            data.errorMessage = locale.value.error.description
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function getOrder() {
    axios.get({
        url: `/manager/orders/repair/view/${data.task.orderId ?? props.order}`,
        cacheTTL: 10
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.order = response
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function goToOrder (order) {
    router.push(`/orders/repair/view/${order.id}`)
}

function beginAddResource (type) {
    data.resourcePage = 0
    data.resourceType = type
}

watch(() => [ data.resourceType, data.resourcePage ], getResources, { immediate: true })

function getResources() {
    if (!data.resourceType) return
    axios.get({
        url: `/resources/${data.resourceType}s/list`,
        params: { page: data.resourcePage },
        cacheTTL: 100
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.resources = response
        }
    })
}

function selectResource (res) {
    data.resources.selected = res
}

function endAddResource () {
    let res = data.resources.selected
    data.task.resources ??= [ ]
    if (res) data.task.resources.push({
        item: res,
        itemId: res.id,
        expectQuantity: 0
    })
    data.resourcePage = undefined
    data.resourceType = undefined
    data.resources = undefined
}

function submitTask() {
    data.errorMessage = undefined
    axios.post({
        url: props.id ? 
            `/manager/tasks/repair/update/${props.id}` :
            `/manager/tasks/repair/create`,
        data: data.task
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.successMessage = locale.value.changesSaved.description
            if (!props.id) router.push(`/tasks/repair/edit/${response.id}`)
            else getTask() 
        }
        else if (response.badFields?.length) {
            data.badFields = arrayToDict(response.badFields)
            data.errorMessage = locale.value.formatBadFields(response.badFields)
        }
        else {
            data.errorMessage = locale.value.saveFailed.description
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function resetTask() {
    data.task = jsonObjectCopy(taskTemplate)
}

</script>
