<template>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe margin-bottom-1">
        <h2 class="flex-grow">My profile</h2>
        <button class="button button-secondary" 
            @click="logoutButtonClick">
            Log out
        </button>
    </div>
    <div class="margin-bottom-1">
        <span>Surname: {{profile.surname}}</span>
    </div>
    <div class="margin-bottom-1">
        <span>Name: {{profile.name}}</span>
    </div>
    <div class="margin-bottom-1">
        <span>Patronym: {{profile.patronym}}</span>
    </div>
    <div class="margin-bottom-1" v-if="profile.orgName">
        <span>Org. name: {{profile.orgName}}</span>
    </div>
    <div class="margin-bottom-1">
        <span>Phone: {{profile.phone}}</span>
    </div>
    <div class="margin-bottom-1">
        <span>E-mail: {{profile.email}}</span>
    </div>
    <div class="pad-1">
        <span class="text-error" v-if="errorMessage">{{errorMessage}}</span>
    </div>
</div>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe">
        <h3 class="flex-grow">My production orders</h3>
        <button class="button button-primary">
            Details
        </button>
    </div>
</div>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe">
        <h3 class="flex-grow">My repair orders</h3>
        <button class="button button-primary">
            Details
        </button>
    </div>
</div>
</template>

<script>

const props = {
    profile: {
        type: Object
    },
    badFields: {
        type: Object,
        default: { }
    },
    errorMessage: {
        type: String
    }
}

function logoutButtonClick() {
    setTimeout(()=> {
        this.$emit("logout")
    }, 200)
}

export default {
    props,
    methods: {
        logoutButtonClick
    },
    emits: [
        "logout"
    ]
}

</script>
