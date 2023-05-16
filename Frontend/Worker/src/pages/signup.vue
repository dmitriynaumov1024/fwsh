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
        url: "/auth/worker/signup",
        data: data
    })
    .then(({ status, data: response }) => {
        if (response.success) {
            signup.success = true
        }
        else {
            if (status == 400 && response.badFields) { 
                signup.badFields = arrayToDict(response.badFields)
                signup.errorMessage = locale.value.formatBadFields(response.badFields, l => l.profile)
            }
            else if (status == 404 && response.badFields) {
                signup.badFields = arrayToDict(response.badFields)
                signup.errorMessage = locale.value.formatNotFound(response.badFields, l => l.profile)
            }
            else if (status == 302 && response.badFields) {
                signup.badFields = arrayToDict(response.badFields)
                signup.errorMessage = locale.value.formatAlreadyExists(response.badFields, l => l.profile)
            } 
            else {
                signup.errorMessage = locale.value.error.description
            }
        }
    })
    .catch(error => {
        signup.errorMessage = locale.value.error.description
        console.error(error)
    })
}

function goToLogin() {
    setTimeout(() => router.replace("/login"), 200)
}

</script>
