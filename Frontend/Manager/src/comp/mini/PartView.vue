<template>
<div class="">
    <table class="kntable stripes">
        <tr>
            <td><b>{{part.item.name}}</b>&ensp;<span class="text-gray">#{{part.id}}</span></td>
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
            <td>{{part.inStock}} / {{part.normalStock}}&ensp;<pencil-icon class="inline icon-1" /></td>
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

</script>
