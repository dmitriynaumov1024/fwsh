<template>
<h2 class="mar-b-1">{{locale.worker.single}}</h2>
<div class="height-15 scroll mar-b-1">
    <template v-for="(worker, id) in workers">
        <radiobox v-model="selected" class="pad-05" :value="worker">
            {{worker.surname}} {{worker.name}} #{{worker.id}}</radiobox>
    </template>
</div>
<div class="flex-stripe flex-pad-1">
    <button class="button button-inline text-gray" 
        @click="()=> emit('click-cancel')">{{locale.action.cancel}}</button>
    <span class="flex-grow"></span>
    <button class="button button-primary"
        @click="()=> emit('click-submit', selected)">
        {{locale.action.confirm}}</button>
</div>
</template>

<script setup>
import { ref, inject } from "vue"
import { Radiobox } from "@common/comp/ctrl"

const props = defineProps({
    workers: Array
})

const selected = ref(null)

const locale = inject("locale")

const emit = defineEmits([
    "click-cancel",
    "click-submit"
])

</script>
