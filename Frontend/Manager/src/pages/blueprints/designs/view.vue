<template>
<Fetch :url="`/manager/designs/view/${id}`" :cacheTTL="20" 
    @load="onLoad" no-default class-error="width-container card pad-1" />
<template v-if="data.design">
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <Crumb to="/blueprints/designs/list?page=0">{{locale.design.plural}}</Crumb>
    <Crumb last>{{data.design.displayName}}</Crumb>
</Bread>
<DesignView :design="data.design"
    :message="data.message"
    @click-edit="editDesign"
    @click-toggle-visible="toggleDesignVisibility" />
</template>
</template>

<script setup>
import { useRouter } from "vue-router" 
import { ref, reactive, inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import DesignView from "@/comp/views/DesignView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    id: [ Number, String ]
})

const data = reactive({ })

function onLoad (design) {
    if (!data.design) data.design = design
}

function editDesign() {
    console.log("Should edit design")
}

function toggleDesignVisibility() {
    data.message = undefined
    let visible = data.design.isVisible
    axios.post({
        url: `/manager/designs/set-visible/${props.id}`,
        params: { visible: !visible }
    })
    .then(({ status, data: response }) => {
        if (status == 200) {
            visible = !visible
            data.design.isVisible = visible
            data.message = locale.value.setVisibleResult[visible]
        }
        else {
            console.log(response)
            data.message = locale.value.error.description
        }
    })
    .catch(error => {
        console.log(error)
        data.message = locale.value.error.description
    })
}

</script>
