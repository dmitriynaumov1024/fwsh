<template>
<header class="header">
    <div class="width-container">
        <div class="flex-stripe pad-05">
            <FwshIcon class="icon-2" />
            <router-link to="/" class="link"><h2>workshop</h2></router-link>
            <span class="flex-grow"></span>
            <MenuButton class="icon-2" :active="headerMenuExpanded" @click="menuButtonClick" />
        </div>
        <nav v-if="headerMenuExpanded" 
            class="flex-stripe pad-05 flex-spacing-1 margin-bottom-05" 
            style="border-top: var(--border)">
            <router-link to="/catalog" class="top-navbar-link">{{ locale.header.nav.catalog }}</router-link>
            <template v-if="profile?.id">
                <router-link to="/profile" class="top-navbar-link">{{ locale.header.nav.profile }}</router-link>
            </template>
            <template v-else>
                <router-link to="/login" class="top-navbar-link">{{ locale.header.nav.login }}</router-link>
                <router-link to="/signup" class="top-navbar-link">{{ locale.header.nav.signup }}</router-link>
            </template>
            <span class="flex-grow"></span>
        </nav>
    </div>
</header>
</template>

<script setup>
import { ref, computed, inject } from "vue"
import FwshIcon from "@/comp/icons/Fwsh.vue" 
import MenuButton from "@/comp/ctrl/Menu.vue"

let headerMenuExpanded = ref(true)

const storage = inject("storage")
const locale = inject("locale")

const profile = computed(() => storage.tmp?.profile)

function menuButtonClick() {
    headerMenuExpanded.value = !(headerMenuExpanded.value)
}

</script>