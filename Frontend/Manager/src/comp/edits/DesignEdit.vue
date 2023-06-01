<template>
    <inputbox type="text" v-model="design.nameKey" 
        :disabled="mode=='edit'" :invalid="badFields?.nameKey">
        {{locale.design.nameKey}}
    </inputbox>
    <inputbox type="text" v-model="design.displayName" 
        :invalid="badFields?.displayName">
        {{locale.design.displayName}}
    </inputbox>
    <groupbox :invalid="badFields?.type">
        <template v-slot:header>{{locale.design.type}}</template>
        <radiobox v-for="type of designTypes" v-model="design.type" :value="type">
            <span>{{locale.furnitureTypes[type]}} ({{type}})</span>
        </radiobox>
    </groupbox>
    <div class="mar-b-1">
        <checkbox v-model="design.isVisible">
            {{locale.design.isVisible}}</checkbox>
    </div>
    <div class="mar-b-1">
        <checkbox v-model="design.isTransformable">
            {{locale.design.isTransformable}}</checkbox>
    </div>
    <groupbox :invalid="badFields?.dimCompact">
        <template v-slot:header>{{locale.design.dimCompact}}</template>
        <inputbox v-for="dimension of ['width', 'length', 'height']"
            type="number" v-model="design.dimCompact[dimension]">
            {{locale.design[dimension]}}, cm
        </inputbox>
    </groupbox>
    <groupbox v-if="design.isTransformable" :invalid="badFields?.dimExpanded">
        <template v-slot:header>{{locale.design.dimExpanded}}</template>
        <inputbox v-for="dimension of ['width', 'length', 'height']"
            type="number" v-model="design.dimExpanded[dimension]">
            {{locale.design[dimension]}}, cm
        </inputbox>
    </groupbox>
    <textbox v-model="design.description" :invalid="badFields?.description">
        {{locale.design.description}}
    </textbox>
    <groupbox>
        <template v-slot:header>{{locale.photo.plural}}</template>
        <div class="preview-photo-gallery">
            <div v-for="url of design.photoUrls">
                <img :src="cdnResolve(url)">
            </div>
            <div v-for="photo of photos">
                <img :src="URL.createObjectURL(photo)" 
                @load="URL.revokeObjectURL(photo)">
            </div>
        </div>
        <div>
            <input type="file" multiple id="input-photos" ref="photoInput" 
                @change="photoInputChanged" class="hidden">
            <label for="input-photos" 
                class="button button-secondary button-block accent-gray">
                + {{locale.action.addPhotos}}
            </label>
        </div>
    </groupbox>
    <div class="mar-b-2">
        <span class="text-error">{{errorMessage}}</span>
    </div>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-inline accent-bad" 
            @click="()=> emit('reset')">{{locale.action.reset}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary" 
            @click="()=> emit('submit')">{{locale.action.submit}}</button>
    </div>
</template>

<script setup>
import { FurnitureTypes } from "@common"
import { cdnResolve } from "@common/utils"
import { ref, reactive, computed, inject } from "vue"
import { Groupbox, Inputbox, Textbox, Checkbox, Radiobox } from "@common/comp/ctrl"

const URL = window.URL

const locale = inject("locale")

const props = defineProps({
    photos: Array,
    design: Object,
    badFields: Object,
    errorMessage: String
})

const mode = computed(()=> (!props.design.id) ? "create" : "edit")

const designTypes = Object.values(FurnitureTypes).filter(v => !!v)

function consoleLogDesign() {
    console.log(JSON.stringify(props.design))
}

const emit = defineEmits([
    "submit",
    "reset",
    "attach-photo",
    "delete-photo",
])

const photoInput = ref(null)

function photoInputChanged() {
    console.log([...photoInput.value.files])
    emit("attach-photo", [...photoInput.value.files])
}

</script>
