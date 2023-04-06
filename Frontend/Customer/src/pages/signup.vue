<template>
<SignupView
    :success="signup.success"
    :badFields="signup.badFields"
    :errorMessage="signup.errorMessage"
    @click-login="goToLogin"
    @click-signup="signupSubmit" /> 
</template>

<script setup>
import { arrayToDict } from "Common"
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import SignupView from "@/comp/SignupView.vue"

const router = useRouter()
const axios = inject("axios")

const signup = reactive({ })

function signupSubmit (data) {
    axios.post({
        url: "/auth/customer/signup",
        data
    })
    .then(({ status, data: response} = axiosresponse) => {
        if (response.success) {
            signup.success = true
        }
        else {
            signup.badFields = arrayToDict(response.badFields)
            signup.errorMessage = response.message
        }
    })
}

function goToLogin() {
    setTimeout(() => router.replace("/login"), 200)
}

</script>
