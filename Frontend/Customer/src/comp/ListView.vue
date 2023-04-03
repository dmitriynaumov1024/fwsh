<template>
<div>
    <template v-for="item of items">
        <component :is="view" 
            v-bind="bind.call ? bind(item) : bind" 
            @click="()=>$emit('click-item', item)" />
    </template>
</div>
</template>

<script>
const props = {
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
}

export default {
    props,
    emits: [
        "click-item"
    ]
}
</script>