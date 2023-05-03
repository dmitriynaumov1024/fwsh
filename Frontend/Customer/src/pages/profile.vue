<template>
<Bread>
    <crumb to="/">fwsh</crumb>
    <crumb last>{{locale.profile.myProfile}}</crumb>
</Bread>
<Fetch url="/customer/profile/view" :cacheTTL="loggedin? 60 : 0"
    @load="onLoad" no-default class-error="width-container card pad-1" />
<ProfileView v-if="profile" 
    :profile="profile"
    @click-logout="profileLogout" 
    @click-production-orders="goToProductionOrders" 
    @click-repair-orders="goToRepairOrders" />
</template>

<script setup>
import { useRouter } from "vue-router"
import { ref, inject, computed } from "vue"
import { Fetch } from "@common/comp/special"
import { Bread, Crumb } from "@common/comp/layout"
import ProfileView from "@/comp/views/ProfileView.vue"

const router = useRouter()

const axios = inject("axios")
const storage = inject("storage")
const locale = inject("locale")

const loggedin = computed(() => storage.tmp?.profile)

const profile = ref(null)

function onLoad (data) { 
    profile.value = data
    storage.tmp.profile = data
}

function profileLogout () {
    axios.post({
        "url": "/auth/customer/logout"
    })
    .then(_ => {
        storage.tmp.profile = null
        router.push("/")
    })
}

function goToProductionOrders () {
    router.push("/orders/production/list?page=0")
}

function goToRepairOrders () {
    router.push("/orders/repair/list?page=0")
}

</script>
