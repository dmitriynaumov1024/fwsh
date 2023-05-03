<template>
<div v-if="success" class="width-container card pad-1 mar-b-2">
    <h2 class="mar-b-1">{{locale.signup.success}}</h2>
    <p class="mar-b-2">
        {{locale.signup.successDescription}}
    </p>
    <div class="button button-block button-primary pad-05 mar-b-1"
        @click="loginButtonClick">
        {{locale.signup.goToLogin}}
    </div>
</div>
<div v-else class="width-container card pad-1 mar-b-2">
    <h2>{{locale.signup.title}}</h2>
    <div class="fancy-input mar-b-1">
        <label for="input-surname">{{locale.profile.surname}}</label>
        <input id="input-surname" type="text" 
            v-model="data.surname" :invalid="!!badFields.surname" />
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-name">{{locale.profile.name}}</label>
        <input id="input-name" type="text" 
            v-model="data.name" :invalid="!!badFields.name" />
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-patronym">{{locale.profile.patronym}}</label>
        <input id="input-patronym" type="text" 
            v-model="data.patronym" :invalid="!!badFields.patronym" />
    </div>
    <div class="fancy-group mar-b-1">
        <header>{{locale.profile.roles}}</header>
        <main>
            <template v-for="role of workerRoles">
                <Checkbox v-model="data.roles" :value="role">{{locale.roles[role] ?? role}}</Checkbox>
            </template>
        </main>
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-phone">{{locale.profile.phone}}</label>
        <input id="input-phone" type="text" 
            v-model="data.phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-email">{{locale.profile.email}}</label>
        <input id="input-email" type="text" 
            v-model="data.email" :invalid="!!badFields.email" />
    </div>
    <div class="fancy-input mar-b-1">
        <label for="input-password">{{locale.profile.password}}</label>
        <input id="input-password" type="password" 
            v-model="data.password" :invalid="!!badFields.password" />
    </div>
    <div class="mar-b-1">
        <span class="text-error">{{errorMessage}}&ensp;</span>
    </div>
    <div class="button button-block button-primary pad-05 mar-b-1"
        @click="signupButtonClick">
        {{locale.action.signup}}
    </div>
    <div class="text-center">
        <span>{{locale.signup.alreadyHaveAccount}}&ensp;</span>
        <button class="button button-inline" @click="loginButtonClick">
            {{locale.action.login}}
        </button>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"
import { Checkbox } from "@common/comp/ctrl"

const locale = inject("locale")

const props = defineProps({
    success: Boolean,
    errorMessage: String,
    badFields: {
        type: Object,
        default: { }
    }
})

const data = reactive({
    surname: "",
    name: "",
    patronym: "",
    roles: [ ],
    phone: "",
    email: "",
    password: ""
})

const emit = defineEmits([
    "click-signup",
    "click-login"
])

const workerRoles = [
    "carpentry",
    "assembly",
    "sewing",
    "upholstery"
]

function signupButtonClick() {
    setTimeout(() => emit("click-signup", data), 200)
}

function loginButtonClick() {
    setTimeout(() => emit("click-login"), 200)
}

</script>