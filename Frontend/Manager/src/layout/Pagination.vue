<template>
<div>
    <slot name="title"></slot>
    <div :class="classPagination">
        <template v-for="item of items">
            <component :is="view" 
                :class="classItem"
                v-bind="bind?.call ? bind(item) : bind" 
                @click="()=>$emit('click-item', item)" />
        </template>
    </div>
    <div class="height-1"></div>
    <div class="flex-stripe">
        <button class="button button-secondary" :disabled="previous==null" @click="()=> $emit('click-previous')">
            &lt;
        </button>
        <span class="flex-grow text-center">
            {{locale.common.page}} {{page}}
        </span>
        <button class="button button-secondary" :disabled="next==null" @click="()=> $emit('click-next')">
            &gt;
        </button>
    </div>
</div>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
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
        default: (item) => item
    },
    classPagination: {
        type: String
    },
    classItem: {
        type: String
    }
})

const emit = defineEmits([
    "click-item",
    "click-previous",
    "click-next"
])

</script>
