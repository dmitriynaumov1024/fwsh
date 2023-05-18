<template>
<div class="width-container card pad-1 mar-b-1">
    <h2 class="mar-b-1">{{locale.task.single}} <span class="text-thin text-gray">#{{task.id}}</span></h2>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.task.workType}}</td>
            <td>{{locale.roles[task.role]}}</td>
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
    <div v-for="res of task.resources" class="mar-b-1">
        <p class="mar-b-05"><b>{{res.item.name}}</b> #{{res.item.id}}</p>
        <table class="kntable align-right border-bottom">
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

const locale = inject("locale")

const props = defineProps({
    task: Object
})

const emit = defineEmits([
    "click-assign",
])

</script>
