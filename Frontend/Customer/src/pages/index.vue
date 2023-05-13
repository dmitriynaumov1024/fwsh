<template>
<div class="width-container pad-1">
    <p class="mar-b-2">Welcome to Furniture workshop!</p>
    <div class="mar-b-1">
        <Selectbox v-model="data.selectedType" :items="furnitureTypes">
            <template v-slot:title>
                {{locale.design.type}}: {{locale.furnitureTypes[data.selectedType]}}
            </template>
            <template v-slot:repeating="{ item }">
                <p class="pad-05">{{locale.furnitureTypes[item]}}</p>
            </template>
        </Selectbox>
    </div>
    <p class="mar-b-1"><b>Buttons</b></p>
    <div class="flex-stripe flex-spacing-1 mar-b-1">
        <button class="button button-primary">Primary</button>
        <button class="button button-secondary">Secondary</button>
        <button class="button button-inline">Inline</button>
        <span class="flex-grow"></span>
    </div>
    <div class="flex-stripe flex-spacing-1 mar-b-1">
        <button class="button button-primary accent-good">Good</button>
        <button class="button button-primary accent-warn">Warning</button>
        <button class="button button-primary accent-error">Error</button>
        <span class="flex-grow"></span>
    </div>
    <div class="flex-stripe flex-spacing-1">
        <button class="button button-secondary accent-good">Good</button>
        <button class="button button-secondary accent-warn">Warning</button>
        <button class="button button-secondary accent-error">Error</button>
        <span class="flex-grow"></span>
    </div>
</div>
</template>

<script setup>
import { FurnitureTypes } from "@common"
import { reactive, ref, inject } from "vue"
import { Selectbox } from "@common/comp/ctrl"

const locale = inject("locale")

const data = reactive({
    selectedType: FurnitureTypes.notSelected
})

const furnitureTypes = Object.values(FurnitureTypes)

</script>
