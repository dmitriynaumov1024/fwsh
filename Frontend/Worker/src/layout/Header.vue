<template>
<HeaderLayout>
    <template v-slot:nav>
        <template v-if="loggedin">
            <router-link to="/profile" class="top-navbar-link">{{ locale.header.nav.profile }}</router-link>
        </template>
        <template v-else>
            <router-link to="/login" class="top-navbar-link">{{ locale.header.nav.login }}</router-link>
            <router-link to="/signup" class="top-navbar-link">{{ locale.header.nav.signup }}</router-link>
        </template>
    </template>
</HeaderLayout>
</template>

<script setup>
import { ref, computed, inject } from "vue"
import HeaderLayout from "@common/layout/Header.vue"

const storage = inject("storage")
const locale = inject("locale")

const loggedin = computed(() => storage.tmp?.profile)

</script>
