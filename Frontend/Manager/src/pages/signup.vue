<template>
<SignupView v-if="signup.isAllowed"
    :success="signup.success"
    :badFields="signup.badFields"
    :errorMessage="signup.errorMessage"
    @click-login="goToLogin"
    @click-signup="signupSubmit" />
<div v-else class="width-container card pad-1 mar-b-1">
    <h2 class="mar-b-05">{{locale.noSignup.title}}</h2>
    <p class="mar-b-2">{{locale.noSignup.description}}</p>
    <div>
        <button class="button button-primary button-block pad-05"
            @click="goToLogin">{{locale.signup.goToLogin}}</button>
    </div>
</div> 
</template>

<script setup>
import { arrayToDict } from "@common/utils"
import { useRouter } from "vue-router"
import { reactive, inject, computed, onMounted } from "vue"
import SignupView from "@/comp/views/SignupView.vue"

const router = useRouter()
const axios = inject("axios")
const locale = inject("locale")

const signup = reactive({ isAllowed: false })

onMounted(signupPing)

function signupPing (data) {
    axios.post({
        url: "/auth/manager/signup",
        data: { }
    })
    .then(({ status }) => {
        signup.isAllowed = (status != 404)
    })
    .catch(error => { })
}

function signupSubmit (data) {
    axios.post({
        url: "/auth/manager/signup",
        data: data
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
