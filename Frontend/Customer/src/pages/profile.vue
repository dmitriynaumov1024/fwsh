<template>
<ProfileView v-if="profile?.id" 
    :profile="profile"
    :errorMessage="profile?.errorMessage" 
    @click-logout="profileLogout" 
    @click-production-orders="goToProductionOrders" 
    @click-repair-orders="goToRepairOrders" />
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import ProfileView from "@/comp/ProfileView.vue"

const router = useRouter()

const axios = inject("axios")
const storage = inject("storage")

const profile = computed(() => storage.tmp?.profile)

onMounted(() => { 
    axios.get({
        url: "/customer/profile/view"
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (status < 299 && response.id) {
            storage.tmp.profile = response
        }
        else {
            storage.tmp.profile = { errorMessage: response.message ?? "Something went wrong" }
            router.replace("/login")
        }
    })
})

function profileLogout () {
    axios.post({
        "url": "/auth/customer/logout"
    })
    .then(_ => {
        storage.tmp.profile = { }
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
