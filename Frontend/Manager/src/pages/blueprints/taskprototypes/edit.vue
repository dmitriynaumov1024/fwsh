<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <template v-if="data.design">
    <Crumb :to="`/blueprints/designs/view/${data.design.id}`">#{{data.design.id}}</Crumb>
    <Crumb :to="`/blueprints/taskprototypes/list?design=${data.design.id}`">{{locale.task.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
    </template>
</Bread>
<div class="width-container card pad-1">
    <DesignView v-if="data.design" :design="data.design"
        clickable @click="goToDesign(data.design)" 
        class="mar-b-1" />
    <TaskPrototypeEdit v-if="data.task" 
        :task="data.task"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetTask"
        @click-save="submitTask" 
        @click-add-resource="beginAddResource"
        @click-add-slot="addSlot" />
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
import DesignView from "@/comp/mini/DesignView.vue"
import TaskPrototypeEdit from "@/comp/edits/TaskPrototypeEdit.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number,
    design: Number
})

let taskTemplate = { 
    description: "...",
    payment: 0, 
    designId: props.design,
    resources: [ ]
}

const data = reactive({
    design: undefined,
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
        getDesign()
        return
    }

    axios.get({
        url: `/manager/taskprototypes/view/${props.id}`,
        cacheTTL: 3
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            taskTemplate = response
            data.task = jsonObjectCopy(taskTemplate)
            if (response.design) data.design = response.design
            else getDesign()
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

function getDesign() {
    axios.get({
        url: `/manager/designs/view/${data.task.designId ?? props.design}`,
        cacheTTL: 10
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.design = response
        }
    })
    .catch(error => {
        console.error(error)
        data.errorMessage = locale.value.error.description
    })
}

function goToDesign (design) {
    router.push(`/blueprints/designs/view/${design.id}`)
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

function addSlot (slotName) {
    data.task.resources ??= [ ]
    if (!slotName || data.task.resources.find(r => r.slotName == slotName)) return
    data.task.resources.push({
        itemId: null,
        slotName: slotName,
        expectQuantity: 0
    })
}

function submitTask() {
    data.errorMessage = undefined
    axios.post({
        url: props.id ? 
            `/manager/taskprototypes/update?id=${props.id}` :
            `/manager/taskprototypes/create?design=${props.design}`,
        data: data.task
    })
    .then(({ status, data: response }) => {
        if (status < 299) {
            data.successMessage = locale.value.changesSaved.description
            if (!props.id) router.push(`/blueprints/taskprototypes/edit/${response.id}`)
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
