<template>
<div>
    <slot name="title"></slot>
    <div v-if="view">
        <template v-for="item of items">
            <component :is="view" 
                v-bind="bind?.call ? bind(item) : bind" 
                @click="()=>$emit('click-item', item)" />
        </template>
    </div>
    <div v-else>
        <template v-for="(item, index) in items">
            <slot name="repeating" :item="item" :index="index"></slot>
        </template>
    </div>
</div>
</template>

<script setup>
const props = defineProps({
    items: {
        type: Array,
        default: [ ],
        required: true
    },
    view: {
        type: Object
    },
    bind: {
        type: undefined,
        default: (item) => item
    }
})

const emit = defineEmits([
    "click-item"
])

</script>