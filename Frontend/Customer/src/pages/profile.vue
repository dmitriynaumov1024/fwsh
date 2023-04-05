<template>
<ProfileView v-if="profile" 
    :profile="profile" 
    :badFields="profile.badFields"
    :errorMessage="profile.errorMessage" 
    @click-logout="profileLogout" 
    @click-production-orders="goToProductionOrders" 
    @click-repair-orders="goToRepairOrders" />
</template>

<script>
import ProfileView from "@/comp/ProfileView.vue"

function arrayToDict (array) {
    let result = { }
    if (array.constructor == Array) 
        for (const key of array) result[key] = true
    return result 
}

function data () {
    return {
        login: { },
        storage: this.$storage
    }
}

const profile = {
    get() {
        return this.storage.profile
    },
    set(value) {
        this.storage.profile = value
    }
}

const loggedIn = {
    get() {
        return !!(this.storage.profile?.id) 
    }
}

function mounted () {
    this.fetchProfile()
}

function fetchProfile () {
    this.$axios.get({
        url: "/customer/profile/view"
    })
    .then(({ status, data: response } = axiosresponse) => {
        if (status < 299 && response.id) {
            this.profile = response
        }
        else {
            this.profile = { }
            this.profile.errorMessage = response.message ?? "Something went wrong"
        }
    })
    .catch(error => {
        console.log("something went wrong")
    })
}

function profileLogout () {
    this.$axios.post({
        "url": "/auth/customer/logout"
    })
    .then(_ => {
        this.profile = { }
        this.$router.push("/")
    })
}

function goToProductionOrders () {
    this.$router.push("/orders/production/list?page=0")
}

function goToRepairOrders () {
    this.$router.push("/orders/repair/list?page=0")
}

export default {
    data,
    mounted,
    computed: {
        profile,
        loggedIn
    },
    methods: {
        fetchProfile,
        profileLogout,
        goToProductionOrders,
        goToRepairOrders
    },
    components: {
        ProfileView
    }
}
</script>