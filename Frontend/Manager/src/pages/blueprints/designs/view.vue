<template>
<template v-if="data.design">
    <Bread :crumbs="[
        { href: '/', text: 'fwsh' },
        { href: '/blueprints', text: locale.blueprint.plural },
        { href: '/blueprints/designs/list?page=0', text: locale.design.plural}
        ]" :last="data.design.displayName" />
    <DesignView :design="data.design"
        @click-edit="editDesign"
        @click-delete="deleteDesign" />
</template>
<div v-else class="width-container text-center pad-1">
    <div v-if="data.notFound">
        <h2 class="margin-bottom-1">Error 404</h2>
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
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    id: [ Number, String ]
})

const data = reactive({})

watch(()=> props.id, getDesign, { immediate: true })

function getDesign() {
    axios.get({
        url: `/manager/designs/view/${props.id}`,
        cacheTTL: 5
    })
    .then(({ status, data: response }) => {
        if (status <= 299) {
            data.design = response
        }
        else if (status == 404) {
            data.notFound = true
        }
        else {
            data.error = true
            console.error(response.message)
        }
    })
    .catch(error => {
        data.error = true
        console.error(error)
    })
}

function editDesign() {
    console.log("Should edit design")
}

function deleteDesign() {
    console.log("Should delete design")
}

</script>