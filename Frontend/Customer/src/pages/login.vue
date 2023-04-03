<template>
<LoginView 
    :badFields="login.badFields" 
    :errorMessage="login.errorMessage" 
    @click-login="loginSubmit"
    @click-signup="gotoSignup" />
</template>

<script>
import LoginView from "@/comp/LoginView.vue"

function data() {
    return {
        login: { }
    }
}

function arrayToDict (array) {
    let result = { }
    if (array.constructor == Array) 
        for (const key of array) result[key] = true
    return result 
}

function loginSubmit ({ phone, password } = data) {
    this.$axios.post({
        url: "/auth/customer/login",
        data: {
            phone,
            password
        }
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (response.success) {
            this.login.errorMessage = undefined
            this.$router.replace("/profile")
        }
        else {
            if ((response.badFields instanceof Array) && response.badFields.length) { 
                this.login.badFields = arrayToDict(response.badFields)
                this.login.errorMessage = response.message ?? `Seems like ${response.badFields[0]} is incorrect`
            }
            else {
                this.login.errorMessage = response.message ?? "Something went wrong"
            }
        }
    })
    .catch(error => {
        console.error(error)
        this.login.errorMessage = "Something went wrong. See console for details"
    })
}

function gotoSignup() {
    this.$router.replace("/signup")
}

export default {
    data,
    methods: {
        loginSubmit,
        gotoSignup
    },
    components: {
        LoginView
    }
}

</script>