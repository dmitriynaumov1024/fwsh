<template>
<LoginView 
    :badFields="login.badFields" 
    :description="props.next && locale.unauthorized.description"
    :errorMessage="login.errorMessage" 
    @click-login="loginSubmit"
    @click-signup="goToSignup" />
</template>

<script setup>
import { arrayToDict } from "@common/utils"
import { useRouter } from "vue-router"
import { ref, reactive, inject } from "vue"
import LoginView from "@/comp/views/LoginView.vue"

const props = defineProps({
    next: String
})

let login = reactive({ })

const router = useRouter()
const axios = inject("axios")
const storage = inject("storage")
const locale = inject("locale")

function loginSubmit ({ phone, password } = data) {
    axios.post({
        url: "/auth/manager/login",
        data: { phone, password }
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (response.success) {
            login.errorMessage = undefined
            login.badFields = undefined
            loadProfile().then(goToNext)
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

function loadProfile () {
    let promise = axios.get({
        url: "/manager/profile/view"
    })
    promise.then(({ status, data: response }) => {
        storage.tmp.profile = response
    })
    return promise
}

function goToNext () {
    setTimeout(() => router.replace(props.next ?? "/profile"), 200)
}

function goToSignup() {
    setTimeout(() => router.replace("/signup"), 200)
}

</script>