<template>
<teleport to="#bread">
<div class="width-container border-bottom">
    <div class="flex-stripe flex-wrap pad-05">
        <template v-for="item in crumbs"> 
            <router-link :to="item.href" class="link text-thin">{{item.text}}</router-link>&ensp;/&ensp;
        </template>
        <span class="link text-thin">{{last}}</span>
        <span class="flex-grow"></span>
    </div>
</div>
</teleport>
</template>

<script setup>
const props = defineProps({
    crumbs: Array,
    last: String
})
</script>
