<template>
<div class="width-container card pad-1 mar-b-2">
    <h2 class="mar-b-1">
        {{locale.login.welcomeBack}}
    </h2>
    <div class="fancy-input mar-b-1">
        <label for="input-phone">{{locale.profile.phone}}</label>
        <input id="input-phone" type="text" 
            v-model="login.phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-password">{{locale.profile.password}}</label>
        <input id="input-password" type="password" 
            v-model="login.password" :invalid="!!badFields.password" /> 
    </div>
    <div class="mar-b-1">
        <span class="text-error">{{errorMessage}}&ensp;</span>
    </div>
    <div class="button button-block button-primary pad-05 mar-b-1"
        @click="loginButtonClick">
        {{locale.action.login}}
    </div>
    <!-- <div class="text-center">
        <span>{{locale.login.haveNoAccount}}&ensp;</span>
        <button class="button button-inline" @click="signupButtonClick">
            {{locale.action.signup}}
        </button>
    </div> -->
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"

const locale = inject("locale")

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
