<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb last>{{locale.repairTask.plural}}</Crumb>
</Bread>
<div class="width-container pad-05">
    <h2 class="mar-b-05">{{locale.repairTask.plural}}</h2>
    <div v-if="props.order" class="flex-stripe flex-pad-1">
        <button class="button button-secondary accent-weak text-strong">{{locale.order.single}} #{{props.order}}</button>
        <span class="flex-grow"></span>
        <router-link v-if="props.order" :to="`/tasks/repair/create?order=${props.order}`" 
            class="button button-primary">+ {{locale.task.single}}</router-link>
    </div>
    <div v-else class="flex-stripe flex-pad-1">
        <template v-for="nextTab of ['list', 'archive']">
            <button v-if="nextTab==props.tab" 
                class="button button-secondary accent-weak text-strong">{{locale.common[nextTab]}}</button>
            <router-link v-else :to="`/tasks/repair/${nextTab}?page=0`" 
                class="button button-primary accent-weak">{{locale.common[nextTab]}}</router-link>
        </template>
        <span class="flex-grow"></span>
    </div>
</div>
<Fetch :url="`/manager/tasks/repair/${props.tab}`"
    :params="{ page: props.page, order: props.order }" :cacheTTL="2"
    class-error="width-container card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
    <Pagination :items="data.items" :page="props.page" 
        :previous="data.previous" :next="data.next"
        @click-previous="()=> goToPage(data.previous)"
        @click-next="()=> goToPage(data.next)"
        class="width-container pad-05 mar-b-1">
        <template v-slot:repeating="{ item }">
            <div class="card-card pad-1 mar-b-1" clickable @click="()=> goToItem(item)">
                <p><b>{{locale.task.single}}</b>&ensp;
                    <router-link :to="`/tasks/repair/view/${item.id}`"
                        class="link">#{{item.id}}</router-link>
                </p>
                <p>{{locale.order.single}}: #{{item.orderId}}</p>
                <p>{{locale.worker.single}}: {{item.workerId ? '#'+item.workerId : '-'}}</p>
                <p>{{item.description}}</p>
            </div>
        </template>
    </Pagination>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, watch } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, Pagination } from "@common/comp/layout"
// import ProductionTaskView from "@/comp/mini/ProductionOrderView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const props = defineProps({
    tab: String,
    page: Number,
    order: Number
})

function goToPage (page) {
    if (page != null && page != undefined)
        router.push(`/tasks/repair/${props.tab}?page=${page}`)
}

function goToItem (item) {
    router.push(`/tasks/repair/view/${item?.id}?tab=${props.tab}`)
}

</script>
