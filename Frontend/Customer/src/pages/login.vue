<template>
<LoginView 
    :badFields="login.badFields" 
    :errorMessage="login.errorMessage" 
    @click-login="loginSubmit"
    @click-signup="goToSignup" />
</template>

<script setup>
import { arrayToDict } from "@common"
import { useRouter } from "vue-router"
import { ref, reactive, inject } from "vue"
import LoginView from "@/comp/LoginView.vue"

let login = reactive({ })

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

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
                login.errorMessage = locale.value.formatBadFields(response.badFields, l => l.profile)
            }
            else {
                login.errorMessage = response.message ?? locale.value.common.somethingWrong
            }
        }
    })
    .catch(error => {
        console.error(error)
        signup.errorMessage = `${locale.value.common.somethingWrong}. ${locale.value.common.seeConsole}`
    })
}

function goToSignup() {
    setTimeout(() => router.replace("/signup"), 200)
}

</script>