<template>
<div>
    <div class="flex-stripe mar-b-1">
        <h2 class="flex-grow">{{locale.task.single}} <span class="text-thin text-gray">#{{task.id}}</span></h2>
        <router-link v-if="canEdit" class="button button-secondary" :to="`/tasks/repair/edit/${task.id}`">
            <pencil-icon class="icon-1 inline" /> {{locale.action.edit}}</router-link>
    </div>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.task.workType}}</td>
            <td>{{locale.roles[task.role]}}</td>
        </tr>
        <tr>
            <td>{{locale.task.payment}}</td>
            <td>{{task.payment}} &#8372;</td>
        </tr>
        <tr clickable @click="()=> emit('click-assign')">
            <td>{{locale.worker.single}}</td>
            <td v-if="task.worker">{{task.worker.surname}} {{task.worker.name}} {{task.worker.patronym}}</td>
            <td v-else-if="task.workerId">#{{task.workerId}}</td>
            <td v-else><button class="button button-inline">{{locale.action.assignWorker}}</button></td>
        </tr>
        <tr>
            <td>{{locale.task.status}}</td>
            <td>{{locale.status[task.status]}}</td>
        </tr>
        <template v-for="actionAt of ['createdAt', 'startedAt', 'finishedAt']">
        <tr v-if="task[actionAt]">
            <td>{{locale.task[actionAt]}}</td>
            <td>{{locale.formatDateTime(task[actionAt])}}</td>
        </tr>
        </template>
        <tr>
            <td>{{locale.task.description}}</td>
            <td>{{task.description}}</td>
        </tr>
    </table>
    <h3 class="mar-b-1">{{locale.resource.plural}}</h3>
    <div v-for="res of task.resources" class="border-left-2 mar-b-1" 
        :class="{ 'text-error': res.item.stored.inStock < (res.expectQuantity - res.actualQuantity) }">
        <p class="mar-b-05"><b>{{res.item.name}}</b> #{{res.item.id}}</p>
        <table class="kntable align-right">
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
                <td>{{res.actualQuantity}}
                    <span v-if="res.item.measureUnit">{{locale.measureUnits[res.item.measureUnit]}}</span></td>
            </tr>
        </table>
    </div>
    <div v-if="!(task.resources?.length)">
        <p class="mar-b-1 text-center"><b>{{locale.noDataYet.title}}</b></p>
        <p class="mar-b-1 text-center">{{locale.noDataYet.description}}</p>
    </div>
</div>
</template>

<script setup>
import { inject } from "vue"
import { PencilIcon } from "@common/comp/icons"

const locale = inject("locale")

const props = defineProps({
    task: Object,
    canEdit: Boolean
})

const emit = defineEmits([
    "click-assign",
])

</script>
