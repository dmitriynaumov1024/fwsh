<template>
<h2 v-if="person.id" class="mar-b-1">{{locale[type]?.single}} <span class="text-thin text-gray">#{{person.id}}</span></h2>
<h2 v-else class="mar-b-1">{{locale[type]?.create}}</h2>

<inputbox type="text" v-model="person.surname" :invalid="badFields.surname">
    {{locale.profile.surname}}</inputbox>
<inputbox type="text" v-model="person.name" :invalid="badFields.name">
    {{locale.profile.name}}</inputbox>
<inputbox type="text" v-model="person.patronym" :invalid="badFields.patronym">
    {{locale.profile.patronym}}</inputbox>

<checkbox v-if="type=='customer'" v-model="person.isOrganization">
    {{locale.profile.isOrganization}}</checkbox>
<inputbox v-if="type=='customer' || type=='supplier'" 
    type="text" v-model="person.orgName" :invalid="badFields.orgName">
    {{locale.profile.orgName}}</inputbox>

<inputbox type="text" v-model="person.phone" :invalid="badFields.phone">
    {{locale.profile.phone}}</inputbox>
<inputbox type="text" v-model="person.email" :invalid="badFields.email">
    {{locale.profile.email}}</inputbox>

<inputbox v-if="person.createdAt" disabled :value="locale.formatDateTime(person.createdAt)">
    {{locale.profile.createdAt}}</inputbox>

<div class="mar-b-2">
    <p class="text-error">{{errorMessage}}</p>
    <p>{{successMessage}}</p>
</div>

<div class="flex-stripe">
    <button class="button button-secondary accent-gray" @click="emit('click-reset')">{{locale.action.reset}}</button>
    <span class="flex-grow"></span>
    <button class="button button-primary" @click="emit('click-submit')">{{locale.action.save}}</button>
</div>
</template>

<script setup>
import { inject } from "vue"
import { Groupbox, Inputbox, Textbox, Checkbox } from "@common/comp/ctrl"

const locale = inject("locale") 

const props = defineProps({
    type: String,
    person: Object,
    successMessage: String,
    errorMessage: String,
    badFields: {
        type: Object,
        default: { }
    }
})

const emit = defineEmits([
    "click-reset",
    "click-submit"
])

</script>