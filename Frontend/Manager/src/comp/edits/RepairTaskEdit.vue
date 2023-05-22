<template>
<h2 class="mar-b-1">
    {{locale.repairTask.single}} 
    <span v-if="task.id" class="text-thin text-gray">#{{task.id}}</span>
</h2>

<groupbox v-if="selectingRole" :invalid="badFields.role">
    <template v-slot:header>
        {{locale.task.role}}
    </template>
    <radiobox v-for="role of WorkerRoles" v-model="task.role" :value="role" 
        @click="()=> { selectingRole = false }">
        <span>{{locale.roles[role]}}</span>
    </radiobox>
</groupbox>
<inputbox v-else :value="locale.roles[task.role]"
    @click="()=> { selectingRole = true }">{{locale.task.role}}</inputbox>

<inputbox v-model="task.payment" :invalid="!!badFields.payment">
    {{locale.task.payment}}, &#8372;</inputbox>

<textbox v-model="task.description" :invalid="!!badFields.description">
    {{locale.task.description}}</textbox>

<div class="mar-b-1"></div>

<div class="mar-b-1">
    <h3>{{locale.resourceUsage.plural}}</h3>
</div>
<div v-for="resource of task.resources" class="mar-b-05">
    <table class="kntable stripes border-left-2">
        <inputbox disabled :value="resource.item.name">
            {{locale.resource.single}}
        </inputbox>
        <inputbox type="number" v-model="resource.expectQuantity">
            {{locale.resourceUsage.expectQuantity}}<template v-if="resource.item.measureUnit">, {{locale.measureUnits[resource.item.measureUnit]}}</template>
        </inputbox>
    </table>
</div>
<div v-if="!(task.resources.length)" class="mar-b-1">
    <p class="text-center mar-b-05"><b>{{locale.noDataYet.title}}</b></p>
    <p class="text-center">{{locale.noDataYet.description}}</p>
</div>
<div class="flex-stripe flex-wrap flex-pad-1 mar-b-1">
    <button class="flex-grow button button-secondary" 
        @click="()=> emit('click-add-resource', 'part')">+ {{locale.part.single}}</button>
    <button class="flex-grow button button-secondary"
        @click="()=> emit('click-add-resource', 'material')">+ {{locale.material.single}}</button>
    <button class="flex-grow button button-secondary"
        @click="()=> emit('click-add-resource', 'fabric')">+ {{locale.fabric.single}}</button>
</div>

<div class="mar-b-1">
    <p v-if="errorMessage" class="text-error">{{errorMessage}}</p>
    <p v-if="successMessage">{{successMessage}}</p>
</div>
<div class="mar-b-1 border-bottom"></div>

<div class="flex-stripe">
    <button class="button button-inline accent-bad" 
        @click="()=> emit('click-reset')">{{locale.action.reset}}</button>
    <span class="flex-grow"></span>
    <button class="button button-primary" 
        @click="()=> emit('click-submit')">{{locale.action.save}}</button>
</div>

</template>

<script setup>
import { MeasureUnits } from "@common"
import { ref, reactive, inject } from "vue"
import { Groupbox, Inputbox, Textbox, Radiobox } from "@common/comp/ctrl"

const locale = inject("locale")

const props = defineProps({
    task: Object,
    badFields: Object,
    errorMessage: String,
    successMessage: String
})

const selectingRole = ref(false)

const emit = defineEmits([
    "click-submit",
    "click-reset",
    "click-add-resource"
])

const WorkerRoles = [
    "unknown",
    "carpentry",
    "assembly",
    "sewing",
    "upholstery",
]

</script>
