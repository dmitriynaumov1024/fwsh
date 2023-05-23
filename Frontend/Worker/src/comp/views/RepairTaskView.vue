<template>
<div class="width-container card pad-1 mar-b-1">
    <h2 class="mar-b-1">{{locale.task.single}} <span class="text-thin text-gray">#{{task.id}}</span></h2>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.task.payment}}</td>
            <td>{{task.payment}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.task.workType}}</td>
            <td>{{locale.roles[task.role]}}</td>
        </tr>
        <tr>
            <td>{{locale.task.description}}</td>
            <td>{{task.description}}</td>
        </tr>
        <tr>
            <td>{{locale.task.status}}</td>
            <td clickable @click="beginSelectStatus">
                <span class="button button-inline">{{locale.status[task.status]}}</span></td>
        </tr>
        <template v-for="actionAt of ['createdAt', 'startedAt', 'finishedAt']">
        <tr v-if="task[actionAt]">
            <td>{{locale.task[actionAt]}}</td>
            <td>{{locale.formatDateTime(task[actionAt])}}</td>
        </tr>
        </template>
    </table>
    <div class="mar-b-1">
        <p v-if="errorMessage" class="text-error">{{errorMessage}}</p>
        <p v-if="successMessage">{{successMessage}}</p>
    </div>
    <h3 class="mar-b-1">{{locale.resource.plural}}</h3>
    <div v-for="res of task.resources" class="border-left-2 mar-b-1">
        <table class="kntable stripes align-right" 
            :class="{ 'text-error': res.item.stored.inStock < (res.expectQuantity - res.actualQuantity) }">
            <tr>
                <td><b>{{res.item.name}}</b> #{{res.item.id}}</td>
            </tr>
            <tr>
                <td>{{locale.resource.inStock}}</td>
                <td>{{res.item.stored.inStock}}
                    <span v-if="res.item.measureUnit">{{locale.measureUnits[res.item.measureUnit]}}</span></td>
            </tr>
            <tr>
                <td>{{locale.resourceUsage.expectQuantity}}</td>
                <td>{{res.expectQuantity}}
                    <span v-if="res.item.measureUnit">{{locale.measureUnits[res.item.measureUnit]}}</span></td>
            </tr>
            <tr>
                <td>{{locale.resourceUsage.actualQuantity}}</td>
                <td><input class="inline-input width-4" v-model="res.actualQuantity" maxlength="8">
                    <span v-if="res.item.measureUnit">&nbsp;{{locale.measureUnits[res.item.measureUnit]}}</span></td>
            </tr>
        </table>
    </div>
    <div class="flex-stripe">
        <button class="button button-secondary accent-weak text-gray"
            @click="()=> emit('click-reset-usage')">{{locale.action.reset}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary"
            @click="()=> emit('click-submit-usage')">{{locale.action.save}}</button>
    </div>
</div>
<Modal v-if="data.selectingStatus">
    <h3 class="mar-b-1">{{locale.task.status}}</h3>
    <div class="mar-b-1">
        <radiobox v-for="status of TaskStatus" v-model="data.status" :value="status"
            class="mar-b-05">
            {{locale.status[status]}}
        </radiobox>
    </div>
    <div class="flex-stripe">
        <button class="button button-inline text-gray"
            @click="()=> endSelectStatus(false)">{{locale.action.cancel}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary" 
            @click="()=> endSelectStatus(true)">{{locale.action.confirm}}</button>
    </div>
</Modal>
</template>

<script setup>
import { TaskStatus } from "@common"
import { reactive, inject } from "vue"
import { Radiobox } from "@common/comp/ctrl" 
import { Modal } from "@common/comp/layout"

const locale = inject("locale")

const props = defineProps({
    task: Object,
    successMessage: String,
    errorMessage: String
})

const emit = defineEmits([
    "click-submit-usage",
    "click-reset-usage",
    "click-set-status"
])

const data = reactive({
    selectingStatus: false,
    status: undefined
})

function beginSelectStatus() {
    data.status = props.task.status
    data.selectingStatus = true
}

function endSelectStatus (success) {
    data.selectingStatus = false
    if (success) emit("click-set-status", data.status)
}

</script>
