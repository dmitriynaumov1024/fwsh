<template>
<div class="mar-b-2">
    <h3 class="mar-b-05">{{resource.item?.name}} <span class="text-thin text-gray">#{{resource.id}}</span></h3>
    <table class="kntable align-right">
        <tr>
            <td>{{locale.resource.externalId}}</td>
            <td>{{resource.externalId}}</td>
        </tr>
        <tr>
            <td>{{locale.resource.normalStock}}</td>
            <td>{{resource.normalStock}} {{locale.measureUnits[resource.item.measureUnit]}}</td>
        </tr>
        <tr>
            <td>{{locale.resource.quantity}}</td>
            <td>{{resource.inStock}} {{locale.measureUnits[resource.item.measureUnit]}}</td>
        </tr>
        <tr>
            <td>{{locale.resource.actualQuantity}}</td>
            <td><input v-model="data.actualQuantity" :invalid="!data.valid" class="inline-input"></td>
        </tr>
    </table>
    <div v-if="data.errorMessage ?? props.errorMessage" class="text-error pad-05">
        {{data.errorMessage ?? props.errorMessage}}
    </div>
</div>
<div class="flex-stripe flex-pad-1">
    <button class="button button-inline accent-gray"
        @click="cancelButtonClick">
        {{locale.action.cancel}}
    </button>
    <span class="flex-grow"></span>
    <button class="button button-primary" 
        @click="submitButtonClick">
        {{locale.action.update}} {{locale.resource.quantity.toLowerCase()}}
    </button>
</div>    
</template>

<script setup>
import { reactive, watch, inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    resource: Object,
    errorMessage: String
})

const data = reactive({
    valid: true,
    errorMessage: undefined,
    actualQuantity: 0
})

watch(()=> props.resource, () => {
    data.actualQuantity = props.resource?.inStock
}, { immediate: true })

const emit = defineEmits([
    'click-submit',
    'click-cancel'
])

function submitButtonClick() {
    console.log(props.resource)
    let quantity = Number(data.actualQuantity)
    if (Number.isNaN(quantity) || data.actualQuantity.length == 0) {
        data.valid = false
        data.errorMessage = locale.value.formatBadFields(["quantity"], l => l.resource)
        return
    }
    data.valid = true
    data.errorMessage = undefined
    emit('click-submit', quantity)
}

function cancelButtonClick() {
    emit('click-cancel')
}

</script>
