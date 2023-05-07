<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb to="/resources">{{locale.resource.plural}}</crumb>
    <crumb last>{{locale.part.plural}}</crumb>
</Bread>
<Fetch url="/resources/parts/list" 
    :params="{ page: props.page, reverse: props.reverse }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
            <h2 class="mar-b-05">{{locale.part.plural}}</h2>
            <div class="flex-stripe flex-pad-1 mar-b-1">
                <button class="button button-secondary accent-weak text-strong">
                    {{locale.common.page}} {{props.page}}
                </button>
                <router-link :to="`/resources/parts/list?page=0&reverse=${!props.reverse}`" 
                    class="button button-secondary accent-weak text-strong">
                    {{locale.common.id}} {{reverse ? '▼' : '▲'}}
                </router-link>
                <span class="flex-grow"></span>
                <router-link to="/resources/parts/create" class="button button-primary">
                    + {{locale.part.single}}
                </router-link>
            </div>
        </template>
        <template v-slot:repeating="{ item }">
            <PartView :part="item" 
                @click-details="()=>goToItem(item)"
                @click-quantity="()=> selectItem(data, item)" 
                class="card-card pad-1 mar-b-1" />
        </template>
    </Pagination>
    <Modal v-if="data.selectedItem">
        <QuantityEdit :resource="data.selectedItem"
            :errorMessage="data.quantityErrorMessage"
            @click-cancel="()=> data.selectedItem = null"
            @click-submit="(newQuantity)=> updateQuantity(data, newQuantity)" />
    </Modal>
    </template>
</Fetch>
</template>

<script setup>
import qs from "qs"
import { useRouter } from "vue-router"
import { reactive, inject } from "vue"
import { nestedObjectAssign } from "@common/utils"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Modal, Pagination } from "@common/comp/layout"
import PartView from "@/comp/mini/PartView.vue"
import QuantityEdit from "@/comp/mini/QuantityEdit.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    page: Number,
    reverse: Boolean
})

function goToPage (page) {
    const query = { page: page, reverse: props.reverse }
    if (page != null) router.push(`/resources/parts/list?${qs.stringify(query)}`)
}

function goToItem (item) {
    router.push(`/resources/parts/edit/${item.id}`)
}

function selectItem (data, item) {
    if (data.selectedItem) return
    console.log("Should go to "+item.id)
    data.selectedItem = item
}

function updateQuantity (data, newQuantity) {
    let item = data.selectedItem
    axios.post({
        url: `/resources/parts/set-quantity/${item.id}`,
        data: newQuantity
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            item.inStock = Number.parseInt(newQuantity)
            item.lastCheckedAt = new Date().toISOString()
            data.quantityErrorMessage = undefined
            data.selectedItem = undefined
        }
        else if (response.badFields) {
            data.quantityErrorMessage = locale.value.formatBadFields(response.badFields, l => l.resource)
        }
        else {
            data.quantityErrorMessage = locale.value.common.somethingWrong
        }
    })
}

function deselectItem () {
    data.selectedItem = undefined
}

</script>
