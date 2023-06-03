<template>
    <inputbox type="text" v-model="design.nameKey" 
        :disabled="mode=='edit'" :invalid="badFields?.nameKey">
        {{locale.design.nameKey}}
    </inputbox>
    <inputbox type="text" v-model="design.displayName" 
        :invalid="badFields?.displayName">
        {{locale.design.displayName}}
    </inputbox>
    <groupbox v-if="selectingType" :invalid="badFields?.type">
        <template v-slot:header>{{locale.design.type}}</template>
        <radiobox v-for="type of designTypes" v-model="design.type" 
            :value="type" @click="()=> {selectingType=false}">
            <span>{{locale.furnitureTypes[type]}} ({{type}})</span>
        </radiobox>
    </groupbox>
    <inputbox v-else :invalid="badFields?.type" :value="locale.furnitureTypes[design.type]" 
        clickable @click="()=> {selectingType=true}">{{locale.design.type}}</inputbox>
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
            {{locale.design[dimension]}}<template v-if="design.dimCompact.measureUnit">,
                {{locale.measureUnits[design.dimCompact.measureUnit]}}</template> 
        </inputbox>
        <groupbox v-if="selectingMeasureUnitCompact">
            <template v-slot:header>{{locale.resource.measureUnit}}</template>
            <div class="flex-stripe flex-pad-1">
                <radiobox v-for="measureUnit of dimMeasureUnits"
                    v-model="design.dimCompact.measureUnit" :value="measureUnit"
                    @click="()=> { selectingMeasureUnitCompact = false }">
                    {{locale.measureUnits[measureUnit]}}
                </radiobox>
                <div class="flex-grow"></div>
            </div>
        </groupbox>
        <inputbox v-else type="text" :value="locale.measureUnits[design.dimCompact.measureUnit]"
            tabindex="-1" clickable @click.prevent="()=> { selectingMeasureUnitCompact = true }">
            {{locale.resource.measureUnit}}
        </inputbox>
    </groupbox>
    <groupbox v-if="design.isTransformable" :invalid="badFields?.dimExpanded">
        <template v-slot:header>{{locale.design.dimExpanded}}</template>
        <inputbox v-for="dimension of ['width', 'length', 'height']"
            type="number" v-model="design.dimExpanded[dimension]">
            {{locale.design[dimension]}}<template v-if="design.dimCompact.measureUnit">,
                {{locale.measureUnits[design.dimCompact.measureUnit]}}</template> 
        </inputbox>
        <groupbox v-if="selectingMeasureUnitExpanded">
            <template v-slot:header>{{locale.resource.measureUnit}}</template>
            <div class="flex-stripe flex-pad-1">
                <radiobox v-for="measureUnit of dimMeasureUnits"
                    v-model="design.dimExpanded.measureUnit" :value="measureUnit"
                    @click="()=> { selectingMeasureUnitExpanded = false }">
                    {{locale.measureUnits[measureUnit]}}
                </radiobox>
                <div class="flex-grow"></div>
            </div>
        </groupbox>
        <inputbox v-else type="text" :value="locale.measureUnits[design.dimExpanded.measureUnit]"
            tabindex="-1" clickable @click.prevent="()=> { selectingMeasureUnitExpanded = true }">
            {{locale.resource.measureUnit}}
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
                <div class="delete" clickable @click="()=> emit('delete-photo', url)">
                    <x-icon class="icon-15" />
                </div>
            </div>
            <div v-for="photo of photos">
                <img :src="URL.createObjectURL(photo)" 
                @load="URL.revokeObjectURL(photo)">
                <div class="delete" clickable @click="()=> emit('delete-photo', photo)">
                    <x-icon class="icon-15" />
                </div>
            </div>
        </div>
        <div>
            <input type="file" multiple id="input-photos" ref="photoInput" 
                @change="photoInputChanged" class="hidden">
            <label for="input-photos" 
                class="button button-secondary button-block accent-weak text-gray">
                + {{locale.action.addPhotos}}
            </label>
        </div>
    </groupbox>
    <div class="mar-b-2">
        <p class="text-error">{{errorMessage}}</p>
        <p>{{successMessage}}</p>
    </div>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-secondary accent-weak text-bad" 
            @click="()=> emit('reset')">{{locale.action.reset}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary" 
            @click="()=> emit('submit')">{{locale.action.submit}}</button>
    </div>
</template>

<script setup>
import { FurnitureTypes, MeasureUnits } from "@common"
import { cdnResolve } from "@common/utils"
import { ref, reactive, computed, inject } from "vue"
import { Groupbox, Inputbox, Textbox, Checkbox, Radiobox } from "@common/comp/ctrl"
import { XIcon } from "@common/comp/icons"

const URL = window.URL

const locale = inject("locale")

const props = defineProps({
    photos: Array,
    design: Object,
    badFields: Object,
    errorMessage: String,
    successMessage: String
})

const mode = computed(()=> (!props.design.id) ? "create" : "edit")

const selectingType = ref(false)
const designTypes = Object.values(FurnitureTypes).filter(v => !!v)

const selectingMeasureUnitCompact = ref(false)
const selectingMeasureUnitExpanded = ref(false)
const dimMeasureUnits = [
    MeasureUnits.millimeters,
    MeasureUnits.centimeters,
    MeasureUnits.meters
]

const emit = defineEmits([
    "submit",
    "reset",
    "attach-photo",
    "delete-photo",
])

const photoInput = ref(null)

function photoInputChanged() {
    emit("attach-photo", [...photoInput.value.files])
}

</script>
