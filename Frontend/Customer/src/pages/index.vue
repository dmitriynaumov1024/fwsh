<template>
<div class="width-container pad-05">
<h2 class="mar-b-1">{{locale.design.plural}}</h2>
<Fetch url="/catalog/designs/list"
    :params="{ page: 0 }" :cacheTTL="600"
    class-error="card pad-1 mar-b-1">
    <template v-slot:default="{ data }">
    <List :items="data.items" class="mar-b-1">
        <template v-slot:repeating="{ item }">
            <DesignView :design="item" @click="()=>goToItem(item)" class="card-card pad-1 mar-b-1" />
        </template>
    </List>
    </template>
</Fetch>
<router-link to="/catalog/designs/list?page=0" class="card link mar-b-1" >
    <h3 class="pad-1">{{locale.common.catalog}} &gt;</h3>
</router-link>
<router-link to="/catalog/fabrics/list?page=0" class="card link mar-b-1" >
    <h3 class="pad-1">{{locale.fabric.plural}} &gt;</h3>
</router-link>
</div>
</template>

<script setup>
import { FurnitureTypes } from "@common"
import { reactive, ref, inject } from "vue"
import { Fetch } from "@common/comp/special"
import { List } from "@common/comp/layout"
import DesignView from "@/comp/mini/DesignView.vue"

const locale = inject("locale")

const data = reactive({
    selectedType: FurnitureTypes.notSelected
})

const furnitureTypes = Object.values(FurnitureTypes)

</script>
