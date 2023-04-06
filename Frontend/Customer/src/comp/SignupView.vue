<template>
<div v-if="success" class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">Success!</h2>
    <p class="margin-bottom-2">
        Your account was created. Now you can log in and get the job done!
    </p>
    <div class="button button-block button-primary pad-05 margin-bottom-1"
        @click="loginButtonClick">
        Go to login
    </div>
</div>
<div v-else class="width-container card pad-1 margin-bottom-2">
    <h2>Create account</h2>
    <div class="fancy-input margin-bottom-1">
        <label for="input-surname">Surname</label>
        <input id="input-surname" type="text" 
            v-model="data.surname" :invalid="!!badFields.surname" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-name">Name</label>
        <input id="input-name" type="text" 
            v-model="data.name" :invalid="!!badFields.name" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-patronym">Patronym</label>
        <input id="input-patronym" type="text" 
            v-model="data.patronym" :invalid="!!badFields.patronym" />
    </div>
    <div class="margin-bottom-1" @click="toggleOrg">
        I am representing organization
    </div>
    <div v-if="data.isOrganization" class="fancy-input margin-bottom-1">
        <label for="input-orgName">Org. name</label>
        <input id="input-orgName" type="text" 
            v-model="data.orgName" :invalid="!!badFields.orgName" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-phone">Phone number</label>
        <input id="input-phone" type="text" 
            v-model="data.phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-email">E-mail</label>
        <input id="input-email" type="text" 
            v-model="data.email" :invalid="!!badFields.email" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-password">Password</label>
        <input id="input-password" type="password" 
            v-model="data.password" :invalid="!!badFields.password" />
    </div>
    <div class="button button-block button-primary pad-05 margin-bottom-1"
        @click="signupButtonClick">
        Sign up
    </div>
    <div class="text-center">
        <span>Already have account?&ensp;</span>
        <button class="button button-inline" @click="loginButtonClick">
            Log in
        </button>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"

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
    orgName: null,
    phone: "",
    email: "",
    password: ""
})

const emit = defineEmits([
    "click-signup",
    "click-login"
])

function toggleOrg() {
    data.isOrganization = !(data.isOrganization)
    if (data.isOrganization) data.orgName = null
}

function signupButtonClick() {
    setTimeout(() => emit("click-signup", data), 200)
}

function loginButtonClick() {
    setTimeout(() => emit("click-login"), 200)
}

</script>