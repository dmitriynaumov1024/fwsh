<template>
    <ProfileView v-if="loggedIn" 
        :profile="profile" 
        :badFields="profile.badFields"
        :errorMessage="profile.errorMessage" 
        @logout="profileLogout" />
    <LoginView v-else 
        :badFields="login.badFields" 
        :errorMessage="login.errorMessage" 
        @submit="loginSubmit" />
</template>

<script>
import LoginView from "@/comp/LoginView.vue"
import ProfileView from "@/comp/ProfileView.vue"

function arrayToDict (array) {
    let result = { }
    if (array.constructor == Array) 
        for (const key of array) result[key] = true
    return result 
}

function data () {
    return {
        loggedIn: false,
        login: { },
        profile: { }
    }
}

function mounted () {
    this.$axios.ping("/customer/profile", {
        expectedFalseStatus: 401
    })
    .then(success => {
        if (success) {
            this.fetchProfile()
        }
    })
    .catch(error => {
        console.log("something went wrong")
    })
}

function fetchProfile () {
    this.$axios.get({
        url: "/customer/profile/view"
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (status < 299 && response.id) {
            this.profile = response
            this.loggedIn = true
        }
        else {
            this.errorMessage = response.message ?? "Something went wrong"
        }
    })
    .catch(error => {
        console.log("something went wrong")
    })
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
            this.errorMessage = undefined
            this.fetchProfile()
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
        this.errorMessage = "Something went wrong. See console for details"
    })
}

function profileLogout () {
    this.$axios.post({
        "url": "/auth/customer/logout"
    })
    .then(_ => {
        this.profile = { }
        this.loggedIn = false
    })
}

export default {
    data,
    mounted,
    methods: {
        fetchProfile,
        loginSubmit,
        profileLogout
    },
    components: {
        LoginView,
        ProfileView
    }
}
</script>