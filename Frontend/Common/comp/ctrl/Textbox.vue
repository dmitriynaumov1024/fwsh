<template>
<div class="fancy-textarea" :invalid="props.invalid">
    <label :for="`input-${$.vnode.key}`">
        <slot></slot>
    </label>
    <textarea :id="`input-${$.vnode.key}`" ref="input"
        :value="value ?? props.modelValue" 
        :disabled="props.disabled" 
        @change="emitUpdate">
    </textarea>
</div>
</template>

<script setup>
import { ref, computed } from "vue"

const props = defineProps({
    invalid: Boolean,
    disabled: Boolean,
    value: undefined,
    modelValue: undefined
})

const emit = defineEmits([
    "update:modelValue"
])

const input = ref(null)

function emitUpdate() {
    let value = input.value.value
    emit("update:modelValue", value)
}

</script>
