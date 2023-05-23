<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/blueprints">{{locale.blueprint.plural}}</Crumb>
    <Crumb to="/blueprints/designs/list?page=0">{{locale.design.plural}}</Crumb>
    <Crumb :to="`/blueprints/designs/view/${props.design}`">#{{props.design}}</Crumb>
    <Crumb last>{{locale.taskPrototype.plural}}</Crumb>
</Bread>
<h2 class="width-container pad-05">{{locale.taskPrototype.plural}}</h2>
<Fetch url="/manager/taskprototypes/list"
    :params="{ design: props.design }" :cacheTTL="4"
    class-error="width-container card pad-1">
    <template v-slot:default="{ data }">
    <List :items="data.items"
        class="width-container pad-05 mar-b-1">
        <template v-slot:title>
        <div class="flex-stripe flex-pad-1 mar-b-1">
            <button class="button button-secondary accent-weak text-strong">
                {{locale.design.single}} #{{props.design}}</button>
            <span class="flex-grow"></span>
            <router-link :to="`/blueprints/taskprototypes/create?design=${props.design}`" 
                class="button button-primary">
                + {{locale.action.create}}</router-link>
        </div>
        </template>
        <template v-slot:repeating="{ item }">
        <div clickable @click="()=> goToItem(item)" class="card-card pad-1 mar-b-1">
            <p><b>{{item.designName}}</b></p>
            <p>{{item.description.slice(0, 90)}}</p>
            <p>{{locale.taskPrototype.precedence}}: {{item.precedence}}</p>
            <p>{{locale.task.workType}}: {{locale.roles[item.role]}}</p>
            <p>{{locale.task.payment}}: {{item.payment}} &#8372;</p>
        </div>
        </template>
    </List>
    </template>
</Fetch>
</template>

<script setup>
import { useRouter } from "vue-router"
import { inject } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb, List } from "@common/comp/layout"
import DesignView from "@/comp/mini/DesignView.vue"

const router = useRouter()
const locale = inject("locale")
const axios = inject("axios")

const props = defineProps({
    design: Number
})

function goToItem(item) {
    if (item.id) 
        router.push(`/blueprints/taskprototypes/edit/${item.id}`)
}

</script>
