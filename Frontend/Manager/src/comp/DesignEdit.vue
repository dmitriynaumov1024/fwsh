<template>
    <div class="fancy-input" :invalid="badFields?.nameKey">
        <label for="input-design-name">{{locale.design.nameKey}}</label>
        <input type="text" id="input-design-name" v-model="design.nameKey" :disabled="mode=='edit'" />
    </div>
    <div class="fancy-input" :invalid="badFields?.displayName">
        <label for="input-design-name">{{locale.design.displayName}}</label>
        <input type="text" id="input-design-name" v-model="design.displayName" />
    </div>
    <div class="fancy-group" :invalid="badFields?.type">
        <header>{{locale.design.type}}</header>
        <main>
            <div v-for="type of designTypes" class="fancy-radio">
                <input type="radio" :id="'radio-design-type-'+type" v-model="design.type" :value="type" />
                <label :for="'radio-design-type-'+type">{{locale.furnitureTypes[type]}} ({{type}})</label>
            </div>
        </main>
    </div>
    <div class="margin-bottom-1">
        <Checkbox v-model="design.isVisible">{{locale.design.isVisible}}</Checkbox>
    </div>
    <div class="margin-bottom-1">
        <Checkbox v-model="design.isTransformable">{{locale.design.isTransformable}}</Checkbox>
    </div>
    <div class="fancy-group" :invalid="badFields?.dimCompact">
        <header>{{locale.design.dimCompact}}</header>
        <main>
            <div v-for="dimension of ['width', 'length', 'height']" class="fancy-input">
                <label :for="'input-design-compact-'+dimension">{{locale.design[dimension]}}, cm</label>
                <input type="number" :id="'input-design-compact-'+dimension" v-model="design.dimCompact[dimension]" />
            </div>
        </main>
    </div>
    <div class="fancy-group" v-if="design.isTransformable" :invalid="badFields?.dimExpanded">
        <header>{{locale.design.dimExpanded}}</header>
        <main>
            <div v-for="dimension of ['width', 'length', 'height']" class="fancy-input">
                <label :for="'input-design-expanded-'+dimension">{{locale.design[dimension]}}, cm</label>
                <input type="number" :id="'input-design-expanded-'+dimension" v-model="design.dimExpanded[dimension]" />
            </div>
        </main>
    </div>
    <div class="fancy-textarea" :invalid="badFields?.description">
        <label for="input-design-description">{{locale.design.description}}</label>
        <textarea id="input-design-description" v-model="design.description"></textarea>
    </div>
    <div class="fancy-group">
        <header>{{locale.photo.plural}}</header>
        <main>
            <div class="preview-photo-gallery">
                <div v-for="url of design.photoUrls"><img :src="cdnResolve(url)"></div>
                <div v-for="photo of photos"><img :src="URL.createObjectURL(photo)" @load="URL.revokeObjectURL(photo)"></div>
            </div>
            <div>
                <input type="file" multiple id="input-photos" ref="photoInput" 
                    @change="photoInputChanged" class="hidden">
                <label for="input-photos" 
                    class="button button-secondary button-block accent-gray">
                    + {{locale.action.addPhotos}}
                </label>
            </div>
        </main>
    </div>
    <div class="margin-bottom-2">
        <span class="text-error">{{errorMessage}}</span>
    </div>
    <div class="flex-stripe flex-pad-1">
        <span class="flex-grow"></span>
        <button class="button button-inline accent-bad" @click="()=> emit('reset')">{{locale.action.clear}}</button>
        <button class="button button-primary" @click="()=> emit('submit')">{{locale.action.submit}}</button>
    </div>
</template>

<script setup>
import { cdnResolve } from "@common/utils"
import { ref, reactive, computed, inject } from "vue"
import Checkbox from "@/comp/ctrl/Checkbox.vue"

const URL = window.URL

const locale = inject("locale")

const props = defineProps({
    photos: Array,
    design: Object,
    badFields: Object,
    errorMessage: String
})

const mode = computed(()=> (!props.design.id) ? "create" : "edit")

const designTypes = [
    "sofa", "corner", "ottoman", "armchair", "pouffe"
]

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
