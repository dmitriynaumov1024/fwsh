<template>
<LoginView 
    :badFields="login.badFields" 
    :errorMessage="login.errorMessage" 
    @click-login="loginSubmit"
    @click-signup="goToSignup" />
</template>

<script setup>
import { arrayToDict } from "Common"
import { useRouter } from "vue-router"
import { ref, reactive, inject } from "vue"
import LoginView from "@/comp/LoginView.vue"

let login = reactive({ })

const router = useRouter()
const axios = inject("axios")

function loginSubmit ({ phone, password } = data) {
    axios.post({
        url: "/auth/customer/login",
        data: { phone, password }
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (response.success) {
            login.errorMessage = undefined
            login.badFields = undefined
            setTimeout(() => router.replace("/profile"), 200)
        }
        else {
            if ((response.badFields instanceof Array) && response.badFields.length) { 
                login.badFields = arrayToDict(response.badFields)
                login.errorMessage = response.message ?? `Seems like ${response.badFields[0]} is incorrect`
            }
            else {
                login.errorMessage = response.message ?? "Something went wrong"
            }
        }
    })
    .catch(error => {
        console.error(error)
        login.errorMessage = "Something went wrong. See console for details"
    })
}

function goToSignup() {
    setTimeout(() => router.replace("/signup"), 200)
}

</script>