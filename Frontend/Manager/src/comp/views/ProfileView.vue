<template>
<div class="width-container card pad-1 margin-bottom-1">
    <div class="flex-stripe margin-bottom-1">
        <h2 class="flex-grow">{{locale.profile.myProfile}}</h2>
        <button class="button button-secondary" 
            @click="()=>$emit('click-logout')">
            {{locale.action.logout}}
        </button>
    </div>
    <table v-if="profile.id" class="kvtable stripes margin-bottom-1">
        <tr>
            <td>{{locale.profile.surname}}</td>
            <td>{{profile.surname}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.name}}</td>
            <td>{{profile.name}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.patronym}}</td>
            <td>{{profile.patronym}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.roles}}</td>
            <td>
                <div v-for="role of profile.roles">
                    <Checkbox checked class="accent-gray">{{locale.roles[role] ?? role}}</Checkbox>
                </div>
            </td>
        </tr>
        <tr>
            <td>{{locale.profile.phone}}</td>
            <td>{{profile.phone}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.email}}</td>
            <td>{{profile.email}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.createdAt}}</td>
            <td>{{locale.formatDate(profile.createdAt)}}</td>
        </tr>
    </table>
    <div class="margin-bottom-1">
        <span class="text-error" v-if="errorMessage">{{errorMessage}}</span>
    </div>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"
import { Checkbox } from "@common/comp/ctrl"

const locale = inject("locale")

const props = defineProps({
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
})

const emit = defineEmits([
    "click-logout"
])

</script>
