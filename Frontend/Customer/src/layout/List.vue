<template>
<div>
    <slot name="title"></slot>
    <template v-for="item of items">
        <component :is="view" 
            v-bind="bind.call ? bind(item) : bind" 
            @click="()=>$emit('click-item', item)" />
    </template>
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
        type: Object,
        default: { },
        required: true
    },
    bind: {
        type: undefined,
        default: (item) => ({ })
    }
})

const emit = defineEmits([
    "click-item"
])

</script>