<template>
<div>
    <slot name="title"></slot>
    <div :class="classPagination">
        <template v-for="item of items">
            <component :is="view" 
                v-bind="bind.call ? bind(item) : bind" 
                @click="()=>$emit('click-item', item)" />
        </template>
    </div>
    <div class="height-1"></div>
    <div class="flex-stripe">
        <button class="button button-secondary" @click="()=> $emit('click-previous')">
            &lt;
        </button>
        <span class="flex-grow text-center">
            Page {{page}}
        </span>
        <button class="button button-secondary" @click="()=> $emit('click-next')">
            &gt;
        </button>
    </div>
</div>
</template>

<script>
const props = {
    page: {
        type: Number
    },
    previous: {
        type: Number
    },
    next: {
        type: Number
    },
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
    },
    classPagination: {
        type: String
    }
}

export default {
    props,
    emits: [
        "click-item",
        "click-previous",
        "click-next"
    ]
}
</script>