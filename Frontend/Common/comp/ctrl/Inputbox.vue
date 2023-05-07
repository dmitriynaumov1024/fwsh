<template>
<div class="fancy-input" :invalid="props.invalid">
    <label :for="`input-${$.vnode.key}`"><slot></slot></label>
    <input :id="`input-${$.vnode.key}`" ref="input"
        :type="nativeInputType"
        :value="value ?? props.modelValue" 
        :disabled="props.disabled" 
        @change="emitUpdate" />
</div>
</template>

<script setup>
import { ref, computed } from "vue"

const props = defineProps({
    type: {
        type: String,
        default: "text"
    },
    invalid: Boolean,
    disabled: Boolean,
    value: undefined,
    modelValue: undefined
})

const emit = defineEmits([
    "update:modelValue"
])

const input = ref(null)

const nativeInputType = computed(() => {
    // just for now
    return props.type == "password" ? "password" : "text" 
})

function emitUpdate() {
    let value = input.value.value, 
        type = props.type.toLowerCase()
    if (type == "text" || type == "string") {
        value = value
    }
    else if (type == "number") {
        value = Number(value)
        if (Number.isNaN(value)) value = props.modelValue
    }
    else if (type == "boolean") {
        value = value == "true"
    }
    emit("update:modelValue", value)
}

</script>
