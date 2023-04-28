<template>
<div class="flex-stripe" @click="toggle">
    <svg class="fancy-checkbox-button" viewBox="0 0 16 16" :checked="thisIsChecked">
        <path v-if="thisIsChecked" stroke="var(--color-accent)" 
            stroke-width="3" fill="none" d="M 3 7 L 8 12 L 18 2" />
        <!-- <template v-if="thisIsChecked">
            <rect x="0" y="0" width="16" height="16" stroke="none" fill="var(--color-accent)" />
            <path stroke="var(--color-back-0)" stroke-width="3" fill="none" d="M 3 7 L 8 12 L 18 2" />
        </template> -->
    </svg>
    <slot></slot>
    <span class="flex-grow"></span>
</div>
</template>

<script setup>
import { computed } from "vue"

const props = defineProps({
    checked: Boolean,
    value: undefined,
    modelValue: [ Boolean, Array ]
})

const thisIsChecked = computed (
    () => props.checked == true 
        || props.modelValue == true 
        || props.modelValue instanceof Array 
        && props.modelValue.includes(props.value)
)

const emit = defineEmits([
    "update:modelValue"
])

function toggle() {
    if (props.modelValue instanceof Array) {
        if (thisIsChecked.value) {
            console.log("remove from array")
            emit("update:modelValue", props.modelValue.filter(a => a != props.value))
        }
        else {  
            console.log("add to array")
            emit("update:modelValue", props.modelValue.concat(props.value))
        }
    }
    else {
        emit("update:modelValue", !props.modelValue)
    }
}

</script>