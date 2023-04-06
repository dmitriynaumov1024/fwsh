<template>
<div class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">
        Welcome back!
    </h2>
    <div class="fancy-input margin-bottom-1">
        <label for="input-phone">Phone number</label>
        <input id="input-phone" type="text" 
            v-model="login.phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-password">Password</label>
        <input id="input-password" type="password" 
            v-model="login.password" :invalid="!!badFields.password" /> 
    </div>
    <div class="margin-bottom-1">
        <span class="text-error">{{errorMessage}}&ensp;</span>
    </div>
    <div class="button button-block button-primary pad-05 margin-bottom-1"
        @click="loginButtonClick">
        Log in
    </div>
    <div class="text-center">
        <span>Have no account yet?&ensp;</span>
        <button class="button button-inline" @click="signupButtonClick">
            Sign up
        </button>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"

const props = defineProps({
    badFields: {
        type: Object,
        default: { }
    },
    errorMessage: {
        type: String
    }
})

const emit = defineEmits([
    "click-login",
    "click-signup"
])

const login = reactive({
    phone: "",
    password: ""
})

function loginButtonClick() {
    setTimeout(() => emit("click-login", login), 200)
}

function signupButtonClick() {
    setTimeout(() => emit("click-signup"), 200)
}

</script>
