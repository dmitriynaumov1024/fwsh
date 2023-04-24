<template>
<template v-if="data.design">
    <Bread :crumbs="[
        { href: '/', text: 'fwsh' },
        { href: '/catalog', text: locale.common.catalog },
        { href: '/catalog/designs/list?page=0', text: locale.design.plural }
        ]" :last="data.design.displayName" />
    <DesignView :design="data.design"
        @click-order="makeOrder" />
</template>
<div v-else class="width-container text-center pad-1">
    <div v-if="data.notFound">
        <h2 class="margin-bottom-1">{{locale.noData.title}}</h2>
        <p>{{locale.design.notFound}}</p>
    </div>
    <div v-else-if="data.error" class="width-container text-center pad-1">
        <p>{{locale.common.somethingWrong}}. {{locale.common.seeConsole}}</p>
    </div>
    <div v-else class="width-container text-center pad-1">
        <p>{{locale.common.loading}}</p>
    </div>
</div>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { reactive, inject, watch } from "vue" 
import Bread from "@/layout/Bread.vue"
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
        url: `/catalog/designs/view/${props.id}`,
        cacheTTL: 600
    })
    .then(({ status, data: response }) => {
        if (response.id) {
            data.design = response
        }
    })
    .catch(error => {
        
    })
}

function makeOrder() {
    console.log("Should make order")
}

</script>
