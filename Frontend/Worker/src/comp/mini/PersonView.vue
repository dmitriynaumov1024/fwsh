<template>
<div class="">
    <p><b>{{person.name}} {{person.surname}}</b> #{{person.id}}</p>
    <p v-if="person.orgName">{{person.orgName}}</p>
    <p>{{person.phone}}</p>
    <p>{{person.email}}</p>
</div>
</template>

<script setup>
const props = defineProps({
    person: Object
})

</script>
