<template>
<DesignView v-if="data.design" :design="data.design" />
<div v-else-if="data.error" class="width-container text-center pad-1">
    <p>{{locale.common.somethingWrong}}. {{locale.common.seeConsole}}</p>
</div>
<div v-else class="width-container text-center pad-1">
    <p>{{locale.common.loading}}</p>
</div>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { reactive, inject, watch } from "vue" 
import DesignView from "@/comp/DesignView.vue"

const router = useRouter()

const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    id: Number
})

const data = reactive({
    design: null
})

watch(() => props.id, getDesign, { immediate: true })

function getDesign () {
    axios.get({
        url: `/catalog/designs/view/${props.id}`
    })
    .then(({ status, data: response }) => {
        if (response.id) {
            data.design = response
        }
    })
    .catch(error => {
        
    })
}
</script>
