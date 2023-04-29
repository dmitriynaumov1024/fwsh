<template>
<div>
    <div class="photo-gallery margin-bottom-1">
        <template v-for="(item, i) in items">
            <slot :active="data.current == i" :item="item"></slot>
        </template>
    </div>
    <div v-if="items?.length" class="photo-gallery-controls flex-stripe accent-gray">
        <button class="button button-secondary" @click="goToPrevious">&lt;</button>
        <span class="text-center flex-grow">{{galleryInfo}}</span>
        <button class="button button-secondary" @click="goToNext">&gt;</button>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, computed } from "vue"

const props = defineProps({
    items: Array
})

const data = reactive({
    current: 0
})

const galleryInfo = computed(() => `${data.current + 1} / ${props.items?.length ?? 0}`)

function goToPrevious() {
    if (data.current > 0) data.current -= 1
}

function goToNext() {
    if (data.current + 1 < props.items?.length) data.current += 1
}

</script>
