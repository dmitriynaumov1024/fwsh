<template>
<table class="kntable stripes">
    <tr v-if="workerId">
        <td>{{locale.worker.single}}</td>
        <td><router-link :to="`/people/workers/view/${paycheck.workerId}`" 
            class="link">#{{paycheck.workerId}}</router-link></td>
    </tr>
    <tr>
        <td>{{locale.paycheck.intervalStart}}</td>
        <td>{{locale.formatDateShort(paycheck.intervalStart)}}</td>
    </tr>
    <tr>
        <td>{{locale.paycheck.intervalEnd}}</td>
        <td>{{locale.formatDateShort(paycheck.intervalEnd)}}</td>
    </tr>
    <tr v-if="paycheck.isReceived">
        <td>{{locale.paycheck.isPaid}}</td>
        <td>{{locale.yesNo[true]}}</td>
    </tr>
    <tr>
        <td>{{locale.paycheck.amount}}</td>
        <td><b>{{paycheck.amount}} &#8372;</b></td>
    </tr>
    <tr v-if="canPay">
       <td>{{locale.paycheck.pay}}</td>
       <td><button class="button button-inline" 
            @click="emit('click-pay')">{{locale.action.confirm}}</button></td> 
    </tr>
</table>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    paycheck: Object,
    canPay: Boolean,
    workerId: undefined
})

const emit = defineEmits([
    'click-pay'
])    

</script>