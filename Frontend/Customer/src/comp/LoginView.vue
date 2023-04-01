<template><div class="width-container card pad-1 margin-bottom-2">
    <h2 class="margin-bottom-1">
        Welcome back!
    </h2>
    <div class="fancy-input margin-bottom-1">
        <label for="input-phone">My phone number</label>
        <input id="input-phone" type="text" 
            v-model="phone" :invalid="!!badFields.phone" />
    </div>
    <div class="fancy-input margin-bottom-1">
        <label for="input-password">My password</label>
        <input id="input-password" type="password" 
            v-model="password" :invalid="!!badFields.password" /> 
    </div>
    <div class="margin-bottom-1">
        <span class="text-error" v-if="errorMessage">{{errorMessage}}</span>
    </div>
    <div class="button button-block button-primary pad-05"
        @click="submitButtonClick">
        Log in
    </div>
</div></template>

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

function submitButtonClick() {
    setTimeout(() => this.$emit("submit", { 
        phone: this.phone, 
        password: this.password
    }), 200)
}

export default {
    props,
    data,
    methods: {
        submitButtonClick
    },
    emits: [
        "submit"
    ]
}
</script>
