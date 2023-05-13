<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/orders/production/list?page=0">{{locale.productionOrder.plural}}</Crumb>
    <Crumb last>{{locale.action.makeOrder}}</Crumb>
</Bread>
<div class="width-container card pad-1">
    <table v-if="data.stage > STAGE_DESIGN" class="kvtable stripes mar-b-1">
        <tr v-if="data.stage > STAGE_DESIGN && data.design">
            <td>{{locale.design.single}}</td>
            <td>{{data.design.displayName}}</td>
        </tr>
        <tr v-if="data.stage > STAGE_DESIGN && data.quantity">
            <td>{{locale.productionOrder.quantity}}</td>
            <td>{{data.quantity}}</td>
        </tr>
        <tr v-if="data.stage > STAGE_COLOR && data.colors?.selected">
            <td>{{locale.color.single}}</td>
            <td>{{data.colors.selected.name}}</td>
        </tr>
        <tr v-else-if="data.stage > STAGE_FABRICTYPE && data.fabricTypes?.selected">
            <td>{{locale.fabricType.single}}</td>
            <td>{{data.fabricTypes.selected.name}}</td>
        </tr>
        <tr v-if="data.stage > STAGE_FABRIC && data.fabrics?.selected">
            <td>{{locale.fabric.single}}</td>
            <td>{{data.fabrics.selected.name}}</td>
        </tr>
        <tr v-if="data.stage > STAGE_DECOR && data.decors?.selected">
            <td>{{locale.decor.single}}</td>
            <td>{{data.decors.selected.name}}</td>
        </tr>
    </table>
    <div v-if="data.stage == STAGE_DESIGN && data.design.id" class="mar-b-2">
        <h2 class="mar-b-05"><span class="text-thin">1.</span> {{locale.design.single}}</h2>
        <DesignView :design="data.design" class="mar-b-2" />
        <div class="flex-stripe flex-pad-1">
            <span class="flex-grow">{{locale.productionOrder.quantity}}</span>
            <button class="button button-secondary accent-gray" 
                :disabled="data.quantity<=1"
                @click="()=> { data.quantity -= 1 }">&ndash;</button>
            <input class="inline-input" style="width: 3rem; text-align: center" v-model="data.quantity" />
            <button class="button button-secondary accent-gray" 
                :disabled="data.quantity>=QUANTITY_LIMIT"
                @click="()=> { data.quantity += 1 }">+</button>
        </div>
    </div>
    <div v-if="data.stage == STAGE_COLOR" class="mar-b-2">
        <Pagination v-if="(data.mode == MODE_FABRICTYPE) && data.fabricTypes"
            :items="data.fabricTypes.items" :page="data.fabricTypes.page"
            :previous="data.fabricTypes.previous" :next="data.fabricTypes.next"
            @click-previous="()=> { data.fabricTypesPage -= 1 }"
            @click-next="()=> { data.fabricTypesPage += 1 }">
            <template v-slot:title>
                <div class="flex-stripe mar-b-05">
                    <h2 class="flex-grow"><span class="text-thin">2.</span> {{locale.fabricType.single}}</h2>
                    <button class="button button-secondary accent-gray" 
                        @click="()=> { data.mode = MODE_COLOR }">{{locale.color.single}}</button>
                </div>
            </template>
            <template v-slot:repeating="{ item }">
                <div class="card-card pad-1 mar-b-1" 
                    :class="{ selected: data.fabricTypes.selected == item }" 
                    @click="()=> { data.fabricTypes.selected = item }">
                    <p><b>{{item.name}}</b></p>
                    <p>{{item.description}}</p>
                </div>
            </template>
        </Pagination>
        <Pagination v-if="(data.mode == MODE_COLOR) && data.colors"
            :items="data.colors.items" :page="data.colors.page"
            :previous="data.colors.previous" :next="data.colors.next"
            @click-previous="()=> { data.colorsPage -= 1 }"
            @click-next="()=> { data.colorsPage += 1 }">
            <template v-slot:title>
                <div class="flex-stripe mar-b-05">
                    <h2 class="flex-grow"><span class="text-thin">2.</span> {{locale.color.single}}</h2>
                    <button class="button button-secondary accent-gray" 
                        @click="()=> { data.mode = MODE_FABRICTYPE }">{{locale.fabricType.single}}</button>
                </div>
            </template>
            <template v-slot:repeating="{ item }">
                <ColorView :color="item" 
                    class="card-card pad-1 mar-b-1" 
                    :class="{ selected: data.colors.selected == item }" 
                    @click="()=> { data.colors.selected = item }"/>
            </template>
        </Pagination>
    </div>
    <div v-else-if="data.stage == STAGE_FABRIC" class="mar-b-2">
        <Pagination v-if="data.fabrics"
            :items="data.fabrics.items" :page="data.fabrics.page"
            :previous="data.fabrics.previous" :next="data.fabrics.next"
            @click-previous="()=> { data.fabricsPage -= 1 }"
            @click-next="()=> { data.fabricsPage += 1 }">
            <template v-slot:title>
                <h2 class="mar-b-05"><span class="text-thin">3.</span> {{locale.fabric.single}}</h2>
            </template>
            <template v-slot:repeating="{ item }">
                <FabricView :fabric="item" 
                    class="card-card pad-1 mar-b-1" 
                    :class="{ selected: data.fabrics.selected == item }"
                    @click="()=> { data.fabrics.selected = item }" />
            </template>
        </Pagination>
    </div>
    <div v-else-if="data.stage == STAGE_DECOR" class="mar-b-2">
        <Pagination v-if="data.decors"
            :items="data.decors.items" :page="data.decors.page"
            :previous="data.decors.previous" :next="data.decors.next"
            @click-previous="()=> { data.decorsPage -= 1 }"
            @click-next="()=> { data.decorsPage += 1 }">
            <template v-slot:title>
                <h2 class="mar-b-05"><span class="text-thin">4.</span> {{locale.decor.single}}</h2>
            </template>
            <template v-slot:repeating="{ item }">
                <MaterialView :mat="item" 
                    class="card-card pad-1 mar-b-1" 
                    :class="{ selected: data.decors.selected == item }"
                    @click="()=> { data.decors.selected = item }" />
            </template>
        </Pagination>
    </div>
    <div class="pad-1">
        <p v-if="data.errorMessage" class="text-error">{{data.errorMessage}}</p>
    </div>
    <div class="flex-stripe flex-pad-1">
        <button class="button button-inline accent-gray" @click="goBack">{{locale.action.back}}</button>
        <span class="fex-grow"></span>
        <button class="button button-primary" @click="goNext">{{locale.action.next}}</button>
    </div>
</div>
</template>

<script setup>
import qs from "qs"
import { useRouter } from "vue-router"
import { reactive, inject, watch, onMounted } from "vue"
import { cdnResolve } from "@common/utils" 
import { Bread, Crumb, Pagination, ImageGallery } from "@common/comp/layout"
import { Inputbox } from "@common/comp/ctrl"
import DesignView from "@/comp/mini/DesignView.vue"
import ColorView from "@/comp/mini/ColorView.vue"
import FabricView from "@/comp/mini/FabricView.vue"
import MaterialView from "@/comp/mini/MaterialView.vue"

const router = useRouter()
const storage = inject("storage")
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    design: String
})

const QUANTITY_LIMIT = 50

const STAGE_DESIGN = 0,
      STAGE_FABRICTYPE = 1,
      STAGE_COLOR = 1,
      STAGE_FABRIC = 2,
      STAGE_DECOR = 3,
      STAGE_SUBMIT = 4

const MODE_COLOR = "color",
      MODE_FABRICTYPE = "fabricType"

const data = reactive({ 
    stage: null,
    mode: MODE_COLOR,
    quantity: 1,
    design: { },
    fabricTypesPage: 0,
    fabricTypes: { },
    colorsPage: 0,
    colors: { },
    fabricsPage: 0,
    fabrics: { },
    decorsPage: 0,
    decors: { },
})

function goBack() {
    if (data.stage > 0) data.stage -= 1
    if (data.stage == STAGE_DECOR && !data.design.decorUsage) data.stage -= 1
}

function goNext() {
    if (data.stage == STAGE_DECOR || (data.stage == STAGE_FABRIC && !data.design.decorUsage)) {
        data.stage = STAGE_SUBMIT
    }
    else {
        data.stage += 1
    }

    if (data.stage > STAGE_SUBMIT) {
        submitOrder()
    }
}

onMounted(() => {
    if (storage.tmp?.profile) {
        data.stage = STAGE_DESIGN
    }
    else {
        let query = qs.stringify({ next: `/orders/production/create?design=${props.design}`})
        router.push(`/login?${query}`)
    }
})

watch(() => props.design, getDesign, { immediate: true })

function getDesign() {
    axios.get({
        url: `/catalog/designs/view/${props.design}`,
        cacheTTL: 600
    })
    .then(({ status, data: response }) => {
        data.design = response
    })
}

watch(() => data.fabricTypesPage, getFabricTypes, { immediate: true })

function getFabricTypes() {
    axios.get({
        url: "/catalog/fabrictypes/list",
        params: { page: data.fabricTypesPage },
        cacheTTL: 600
    })
    .then(({ status, data: response }) => {
        data.fabricTypes = response
    })
}

watch(() => data.colorsPage, getColors, { immediate: true })

function getColors() {
    axios.get({
        url: "/catalog/colors/list",
        params: { page: data.colorsPage, slot: "fabric" },
        cacheTTL: 600
    })
    .then(({ status, data: response }) => {
        data.colors = response
    })
}

watch(() => [data.fabricsPage, data.colors?.selected, data.fabricTypes?.selected], getFabrics, { immediate: true })

function getFabrics() {
    axios.get({
        url: "/catalog/fabrics/list",
        params: { 
            page: data.fabricsPage, 
            color: data.colors.selected?.id,
            fabrictype: data.fabricTypes.selected?.id 
        }
    })
    .then(({ status, data: response }) => {
        data.fabrics = response
    })
}

watch(() => data.decorsPage, getDecors, { immediate: true })

function getDecors() {
    axios.get({
        url: "/catalog/materials/list",
        params: { page: data.decorsPage }
    })
    .then(({ status, data: response }) => {
        data.decors = response
    })
}

function submitOrder() {
    axios.post({
        url: "/customer/orders/production/create",
        data: {
            quantity: data.quantity,
            designId: data.design.id,
            fabricId: data.fabrics.selected?.id,
            decorId: data.decors.selected?.id
        }
    })
    .then(({ status, data: response }) => {
        if (status < 299 && response.id) {
            router.replace(`/orders/production/view/${response.id}`)
        }
        else {
            data.errorMessage = locale.error.description
        }
    })

}

</script>
