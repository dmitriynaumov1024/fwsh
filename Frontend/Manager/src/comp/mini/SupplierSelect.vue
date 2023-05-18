<template>
<h2 class="mar-b-1">{{locale.supplier.single}}</h2>
<div class="height-15 scroll mar-b-1">
    <template v-for="(supplier, id) in suppliers">
        <radiobox v-model="selected" class="pad-05" :value="supplier">
            <div><p>{{supplier.surname}} {{supplier.name}}</p>
                <p>{{supplier.orgName}}, {{supplier.phone}}</p>
            </div>
        </radiobox>
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
    suppliers: Array,
    selection: Object
})

const selected = ref ( 
    props.selection ? 
    props.suppliers.find(s => s.id == props.selection.id) :
    null
)

const locale = inject("locale")

const emit = defineEmits([
    "click-cancel",
    "click-submit"
])

</script>
