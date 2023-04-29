<template>
<SignupView
    :success="signup.success"
    :badFields="signup.badFields"
    :errorMessage="signup.errorMessage"
    @click-login="goToLogin"
    @click-signup="signupSubmit" /> 
</template>

<script setup>
import { arrayToDict } from "@common/utils"
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import SignupView from "@/comp/views/SignupView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

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
            signup.errorMessage = locale.value.formatBadFields(response.badFields, l => l.profile)
        }
    })
    .catch(error => {
        signup.errorMessage = `${locale.value.common.somethingWrong}. ${locale.value.common.seeConsole}`
        console.error(error)
    })
}

function goToLogin() {
    setTimeout(() => router.replace("/login"), 200)
}

</script>
