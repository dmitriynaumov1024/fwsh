<template>
<div class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">
        Welcome back!
    </h2>
    <div class="fancy-input margin-bottom-1">
        <label for="input-phone">Phone number</label>
        <input id="input-phone" type="text" 
            v-model="phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-password">Password</label>
        <input id="input-password" type="password" 
            v-model="password" :invalid="!!badFields.password" /> 
    </div>
    <div class="margin-bottom-1">
        <span class="text-error">{{errorMessage}}&ensp;</span>
    </div>
    <div class="button button-block button-primary pad-05 margin-bottom-1"
        @click="loginButtonClick">
        Log in
    </div>
    <div class="text-center">
        <span>Have no account yet? &ensp;</span>
        <button class="button button-inline" 
            @click="signupButtonClick">
            Sign up
        </button>
    </div>
</div>
</template>

<script>

const props = {
    badFields: {
        type: Object,
        default: { }
    },
    errorMessage: {
        type: String
    }
}

function data() {
    return {
        phone: "",
        password: ""
    }
}

function loginButtonClick() {
    setTimeout(() => this.$emit("click-login", { 
        phone: this.phone, 
        password: this.password
    }), 200)
}

function signupButtonClick() {
    setTimeout(() => this.$emit("click-signup"), 200)
}

export default {
    props,
    data,
    methods: {
        loginButtonClick,
        signupButtonClick
    },
    emits: [
        "click-login",
        "click-signup"
    ]
}
</script>
