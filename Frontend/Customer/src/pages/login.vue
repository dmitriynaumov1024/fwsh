<template>
<LoginView 
    :badFields="login.badFields" 
    :errorMessage="login.errorMessage" 
    @click-login="loginSubmit"
    @click-signup="goToSignup" />
</template>

<script setup>
import qs from "qs"
import { arrayToDict } from "@common/utils"
import { useRouter } from "vue-router"
import { ref, reactive, inject } from "vue"
import LoginView from "@/comp/views/LoginView.vue"

const props = defineProps({
    next: String
})

let login = reactive({ })

const router = useRouter()
const storage = inject("storage")
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
            axios.get({
                url: "/customer/profile/view"
            })
            .then(({ status, data: response }) => {
                storage.tmp.profile = response
                router.replace(props.next ?? "/profile")
            })
        }
        else {
            if (status == 400 && response.badFields) { 
                login.badFields = arrayToDict(response.badFields)
                login.errorMessage = locale.value.formatBadFields(response.badFields, l => l.profile)
            }
            else if (status == 404 && response.badFields) {
                login.badFields = arrayToDict(response.badFields)
                login.errorMessage = locale.value.formatNotFound(response.badFields, l => l.profile)
            }
            else if (status == 302 && response.badFields) {
                login.badFields = arrayToDict(response.badFields)
                login.errorMessage = locale.value.formatAlreadyExists(response.badFields, l => l.profile)
            } 
            else {
                login.errorMessage = locale.value.error.description
            }
        }
    })
    .catch(error => {
        console.error(error)
        signup.errorMessage = `${locale.value.common.somethingWrong}. ${locale.value.common.seeConsole}`
    })
}

function goToSignup() {
    const query = qs.stringify({ next: props.next })
    setTimeout(() => router.replace(`/signup?${query}`), 200)
}

</script>