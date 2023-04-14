<template>
<Bread :crumbs="[{ href: '/', text: 'fwsh' }]" 
    :last="locale.profile.myProfile" />
<ProfileView v-if="profile.id" 
    :profile="profile"
    :errorMessage="profile.errorMessage" 
    @click-logout="profileLogout" />
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import Bread from "@/layout/Bread.vue"
import ProfileView from "@/comp/ProfileView.vue"

const router = useRouter()

const axios = inject("axios")
const storage = inject("storage")
const locale = inject("locale")

const profile = computed(() => storage.tmp?.profile ?? { })

onMounted(() => { 
    axios.get({
        url: "/manager/profile/view"
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
        url: "/auth/manager/logout"
    })
    .then(_ => {
        storage.tmp.profile = { }
        router.push("/")
    })
}

</script>
