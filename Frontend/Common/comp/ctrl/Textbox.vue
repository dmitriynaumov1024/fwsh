<template>
<div class="fancy-textarea" :invalid="props.invalid">
    <label :for="`input-${idInternal}`">
        <slot></slot>
    </label>
    <textarea :id="`input-${idInternal}`" ref="input"
        :value="props.value ?? props.modelValue" 
        :disabled="props.disabled"
        :tabindex="props.tabindex" 
        @change="emitUpdate">
    </textarea>
</div>
</template>

<script setup>
import { ref, computed } from "vue"

const props = defineProps({
    invalid: Boolean,
    disabled: Boolean,
    name: String,
    tabindex: undefined,
    value: undefined,
    modelValue: undefined
})

const emit = defineEmits([
    "update:modelValue"
])

const idInternal = ref(null)

onMounted(() => {
    if (props.name) {
        idInternal.value = props.name
    }
    else {
        window._idInternal = (window._idInternal ?? 0) + 1
        idInternal.value = window._idInternal
    }
})

const input = ref(null)

function emitUpdate() {
    let value = input.value.value
    emit("update:modelValue", value)
}

</script>
