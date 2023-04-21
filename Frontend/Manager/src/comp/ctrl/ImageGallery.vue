<template>
<div>
    <div class="photo-gallery margin-bottom-1" ref="photoGallery">
        <slot></slot>
    </div>
    <div class="photo-gallery-controls flex-stripe accent-gray">
        <button class="button button-secondary" @click="goToPrevious">&lt;</button>
        <span class="text-center flex-grow">{{galleryInfo}}</span>
        <button class="button button-secondary" @click="goToNext">&gt;</button>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, computed, watch } from "vue"

const data = reactive({
    current: 0
})

const photoGallery = ref(null)

const galleryInfo = computed(() => `${data.current + 1} / ${photoGallery.value?.childElementCount}`)

watch(() => data.current, (index) => {
    setTimeout(() => {
        if (photoGallery.value == null) return
        for (const img of photoGallery.value.children) img.classList.remove("visible")
        photoGallery.value.children[index].classList.add("visible")
    }, 60)
}, { immediate: true })

function goToPrevious() {
    if (data.current > 0) data.current -= 1
}

function goToNext() {
    if (data.current + 1 < photoGallery.value?.childElementCount) data.current += 1
}

</script>