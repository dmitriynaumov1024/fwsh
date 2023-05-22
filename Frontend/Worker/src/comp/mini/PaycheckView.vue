<template>
<table class="kntable stripes">
    <tr>
        <td>{{locale.paycheck.intervalStart}}</td>
        <td>{{locale.formatDateShort(paycheck.intervalStart)}}</td>
    </tr>
    <tr>
        <td>{{locale.paycheck.intervalEnd}}</td>
        <td>{{locale.formatDateShort(paycheck.intervalEnd)}}</td>
    </tr>
    <tr>
        <td>{{locale.paycheck.isPaid}}</td>
        <td>{{locale.yesNo[paycheck.isReceived]}}</td>
    </tr>
    <tr>
        <td>{{locale.paycheck.amount}}</td>
        <td><b>{{paycheck.amount}} &#8372;</b></td>
    </tr>
</table>
</template>

<script setup>
import { inject } from "vue"

const locale = inject("locale")

const props = defineProps({
    paycheck: Object
})

</script>