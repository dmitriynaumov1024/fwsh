<template>
<h2 class="mar-b-1">
    {{locale.taskPrototype.single}} 
    <span v-if="task.id" class="text-thin text-gray">#{{task.id}}</span>
</h2>

<inputbox v-model="task.precedence" :invalid="!!badFields.precedence">
    {{locale.taskPrototype.precedence}}</inputbox>

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
<div v-for="resource of task.resources" class="mar-b-05 border-left-2">
    <template v-if="resource.item">
    <inputbox disabled :value="resource.item.name">
        {{locale.resource.single}}
    </inputbox>
    <inputbox type="number" v-model="resource.expectQuantity">
        {{locale.resourceUsage.expectQuantity}}<template v-if="resource.item.measureUnit">, {{locale.measureUnits[resource.item.measureUnit]}}</template>
    </inputbox>
    </template>
    <template v-else>
    <inputbox disabled :value="locale.slotNames[resource.slotName]">
        {{locale.resource.slotName}}
    </inputbox>
    <inputbox type="number" v-model="resource.expectQuantity">
        {{locale.resourceUsage.expectQuantity}}, {{locale.measureUnits.m2}}
    </inputbox>
    </template>
</div>
<div v-if="!(task.resources.length)" class="mar-b-1">
    <p class="text-center mar-b-05"><b>{{locale.noDataYet.title}}</b></p>
    <p class="text-center">{{locale.noDataYet.description}}</p>
</div>
<div class="flex-stripe flex-pad-1 mar-b-1">
    <button v-for="resType of ['part', 'material', 'fabric']"
        class="flex-grow button button-secondary accent-weak text-strong" 
        @click="()=> emit('click-add-resource', resType)">
        + {{locale[resType].single}}
    </button>
</div>
<div class="flex-stripe flex-pad-1 mar-b-1">
    <button v-for="slot of ['decor', 'fabric']" 
        class="flex-grow button button-secondary accent-warn text-strong"
        @click="()=> emit('click-add-slot', slot)">
        + {{locale.resource.slotName}} ({{locale.slotNames[slot]}})
    </button>
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
        @click="()=> emit('click-save')">{{locale.action.save}}</button>
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
    "click-save",
    "click-reset",
    "click-add-resource",
    "click-add-slot"
])

const WorkerRoles = [
    "unknown",
    "carpentry",
    "assembly",
    "sewing",
    "upholstery",
]

</script>
