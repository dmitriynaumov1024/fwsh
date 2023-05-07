<template>
<div class="mini-resource-view">
    <table class="kntable stripes">
        <tr>
            <td clickable @click="()=> emit('click-details')">
                <b>{{part.item.name}}</b>&ensp;<span class="text-gray">#{{part.id}}</span></td>
        </tr>
        <tr>
            <td>{{locale.resource.externalId}}</td>
            <td>{{part.externalId}}</td>
        </tr>
        <tr>
            <td>{{locale.resource.pricePerUnit}}</td>
            <td>{{part.item.pricePerUnit}} &#8372;</td>
        </tr>
        <tr>
            <td>{{locale.resource.inStock}}</td>
            <td><p>{{part.inStock}} / {{part.normalStock}}</p>
                <button class="button button-inline" @click="()=> emit('click-quantity')">
                    <pencil-icon class="inline icon-1" /> {{locale.action.update}}
                </button>
            </td>
        </tr>
    </table>
</div>    
</template>

<script setup>
import { inject } from "vue"
import { PencilIcon } from "@common/comp/icons"

const props = defineProps({
    part: Object
})

const locale = inject("locale")

const emit = defineEmits([
    "click-quantity",
    "click-details"
])

</script>
