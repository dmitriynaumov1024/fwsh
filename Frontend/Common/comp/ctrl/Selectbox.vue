<template>
<div clickable class="fancy-selectbox" :active="active">
    <header class="flex-stripe" @click="toggleActive">
        <slot name="title"></slot><span v-if="!active">&#9662;</span>
    </header>
    <main>
        <div v-for="item of items" @click="()=> select(item)" :class="{ selected: modelValue==item }">
            <slot name="repeating" :item="item"></slot>
        </div>
    </main>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"

const props = defineProps({
    items: Array,
    getValue: Function,
    modelValue: undefined
})

const emit = defineEmits([
    "update:modelValue"
])

const active = ref(false)

function toggleActive() {
    active.value = !active.value
}

function select(item) {
    active.value = false
    emit("update:modelValue", props.getValue ? props.getValue(item) : item)
}

</script>
