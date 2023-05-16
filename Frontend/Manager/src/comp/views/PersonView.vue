<template>
<div class="width-container card pad-1 mar-b-1">
    <h2 class="flex-grow">{{locale[props.type]?.single}}</h2>
    <table class="kvtable stripes mar-b-1">
        <tr>
            <td>{{locale.common.id}}</td>
            <td>#{{person.id}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.surname}}</td>
            <td>{{person.surname}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.name}}</td>
            <td>{{person.name}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.patronym}}</td>
            <td>{{person.patronym}}</td>
        </tr>
        <tr v-if="person.orgName">
            <td>{{locale.profile.orgName}}</td>
            <td>{{person.orgName}}</td>
        </tr>
        <tr v-if="person.roles">
            <td>{{locale.profile.roles}}</td>
            <td>
                <div v-for="role of person.roles">
                    <Checkbox checked class="accent-gray">{{locale.roles[role] ?? role}}</Checkbox>
                </div>
            </td>
        </tr>
        <tr>
            <td>{{locale.profile.phone}}</td>
            <td>{{person.phone}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.email}}</td>
            <td>{{person.email}}</td>
        </tr>
        <tr>
            <td>{{locale.profile.createdAt}}</td>
            <td>{{locale.formatDate(person.createdAt)}}</td>
        </tr>
    </table>
</div>
</template>

<script setup>
import { ref, reactive, inject } from "vue"
import { Checkbox } from "@common/comp/ctrl"

const locale = inject("locale")

const props = defineProps({
    person: Object,
    type: String
})

const emit = defineEmits([ ])

</script>
