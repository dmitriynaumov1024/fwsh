<template>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe margin-bottom-1">
        <h2 class="flex-grow">My profile</h2>
        <button class="button button-secondary" 
            @click="logoutButtonClick">
            Log out
        </button>
    </div>
    <table class="kvtable stripes margin-bottom-1">
        <tr>
            <td>Surname</td>
            <td>{{profile.surname}}</td>
        </tr>
        <tr>
            <td>Name</td>
            <td>{{profile.name}}</td>
        </tr>
        <tr>
            <td>Patronym</td>
            <td>{{profile.patronym}}</td>
        </tr>
        <tr v-if="profile.orgName">
            <td>Org. name</td>
            <td>{{profile.orgName}}</td>
        </tr>
        <tr>
            <td>Phone</td>
            <td>{{profile.phone}}</td>
        </tr>
        <tr>
            <td>E-mail</td>
            <td>{{profile.email}}</td>
        </tr>
        <tr>
            <td>Discount percent</td>
            <td>{{profile.discountPercent}}%</td>
        </tr>
        <tr>
            <td>Signup date</td>
            <td>{{toDateString(profile.createdAt)}}</td>
        </tr>
    </table>
    <div class="margin-bottom-1">
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
        this.$emit("click-logout")
    }, 200)
}

let dateFormat = Intl?.DateTimeFormata ? new Intl.DateTimeFormat("uk-UA", {
    day: "numeric",
    month: "long",
    year: "numeric"
})
: ({
    format(date) {
        return date.toDateString()
    }
})

function toDateString (date) {
    date = new Date(date)
    return dateFormat.format(date)
}

export default {
    props,
    methods: {
        logoutButtonClick,
        toDateString
    },
    emits: [
        "click-logout"
    ]
}

</script>
