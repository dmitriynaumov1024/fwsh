<template>
<HeaderLayout />
<MainLayout />
<FooterLayout />
</template>

<script setup>
import { onMounted, inject } from "vue"
import HeaderLayout from "@/layout/Header.vue"
import MainLayout from "@/layout/Main.vue"
import FooterLayout from "@/layout/Footer.vue"

const axios = inject("axios")
const storage = inject("storage")

onMounted(() => { 
    axios.get({
        url: "/customer/profile/view"
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (status < 299 && response.id) {
            storage.tmp.profile = response
        }
    })
})

</script>
