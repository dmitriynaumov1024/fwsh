<template>
<h2 class="mar-b-05">{{locale.worker.single}} <span class="text-thin text-gray">#{{worker.id}}</span></h2>
<table class="kvtable stripes mar-b-1">
    <tr>
        <td>{{locale.profile.surname}}</td>
        <td>{{worker.surname}}</td>
    </tr>
    <tr>
        <td>{{locale.profile.name}}</td>
        <td>{{worker.name}}</td>
    </tr>
    <tr>
        <td>{{locale.profile.patronym}}</td>
        <td>{{worker.patronym}}</td>
    </tr>
    <tr>
        <td>{{locale.profile.roles}}</td>
        <td><div v-for="role of worker.roles" class="mar-b-05">
            <Checkbox checked class="accent-gray">{{locale.roles[role] ?? role}}</Checkbox>
        </div></td>
    </tr>
    <tr>
        <td>{{locale.profile.phone}}</td>
        <td>{{worker.phone}}</td>
    </tr>
    <tr>
        <td>{{locale.profile.email}}</td>
        <td>{{worker.email}}</td>
    </tr>
    <tr>
        <td>{{locale.profile.createdAt}}</td>
        <td>{{locale.formatDate(worker.createdAt)}}</td>
    </tr>
</table>
<h3 class="mar-b-05">{{locale.paycheck.plural}}</h3>
<template v-for="paycheck of worker.paychecks">
    <PaycheckView :paycheck="paycheck" :canPay="!paycheck.isPaid" 
        @click-pay="()=> emit('click-confirm-payment', paycheck)"
        class="border-bottom mar-b-05" />
</template>
<div v-if="!(worker.paychecks?.length)" class="pad-1 mar-b-1">
    <p class="text-center mar-b-05"><b>{{locale.noDataYet.title}}</b></p>
    <p class="text-center">{{locale.noDataYet.description}}</p>
</div>
<div class="mar-b-1">
    <p v-if="errorMessage" class="text-error">{{errorMessage}}</p>
    <p v-else-if="successMessage">{{successMessage}}</p>
</div>
<div class="flex-stripe flex-wrap">
    <button class="button button-primary"
        @click="()=> emit('click-create-paycheck')">
        + {{locale.paycheck.single}}
    </button>
    <button class="button button-secondary"
        @click="()=> { data.showCreateBonus = true }">
        + {{locale.paycheckBonus.single}}
    </button>
</div>
<Modal v-if="data.showCreateBonus">
    <h3 class="mar-b-1">{{locale.paycheckBonus.create}}</h3>
    <p class="mar-b-1">{{worker.surname}} {{worker.name}} {{worker.patronym}}</p>
    <table class="kntable align-right mar-b-1">
        <tr>
            <td>{{locale.paycheck.amount}}</td>
            <td><input class="inline-input" v-model="data.bonusAmount"></td>
        </tr>
    </table>
    <div class="flex-stripe">
        <button class="button button-inline accent-gray" 
            @click="()=> endCreateBonus()">{{locale.action.cancel}}</button>
        <span class="flex-grow"></span>
        <button class="button button-primary" 
            @click="()=> endCreateBonus(data.bonusAmount)">{{locale.action.confirm}}</button>
    </div>
</Modal>
</template>

<script setup>
import { ref, reactive, inject } from "vue"
import { Checkbox } from "@common/comp/ctrl"
import { Modal } from "@common/comp/layout"
import PaycheckView from "@/comp/mini/PaycheckView.vue"

const locale = inject("locale")

const props = defineProps({
    worker: Object,
    errorMessage: String,
    successMessage: String
})

const data = reactive({
    showCreateBonus: false,
    bonusAmount: 0
})

const emit = defineEmits([
    "click-confirm-payment",
    "click-create-paycheck",
    "click-create-bonus"
])

function endCreateBonus(amount) {
    data.showCreateBonus = false
    if (amount) emit("click-create-bonus", amount)
}

</script>
