<template>
<div class="fancy-input" :invalid="props.invalid">
    <label :for="`input-${idInternal}`"><slot></slot></label>
    <input :id="`input-${idInternal}`" ref="input"
        :type="nativeInputType"
        :value="props.value ?? props.modelValue" 
        :disabled="props.disabled"
        :tabindex="props.tabindex" 
        @change="emitUpdate" />
</div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue"

const props = defineProps({
    type: {
        type: String,
        default: "text"
    },
    invalid: Boolean,
    disabled: Boolean,
    name: String,
    tabindex: undefined,
    value: undefined,
    modelValue: undefined
})

const idInternal = ref(null)

const emit = defineEmits([
    "update:modelValue"
])

const input = ref(null)

const nativeInputType = computed(() => {
    // just for now
    return props.type == "password" ? "password" : "text" 
})

onMounted(() => {
    if (props.name) {
        idInternal.value = props.name
    }
    else {
        window._idInternal = (window._idInternal ?? 0) + 1
        idInternal.value = window._idInternal
    }
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
