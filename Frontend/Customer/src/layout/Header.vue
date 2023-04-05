<template>
<header class="header">
    <div class="width-container">
        <div class="flex-stripe pad-05">
            <FwshIcon class="icon-2" />
            <router-link to="/" class="link"><h2>workshop</h2></router-link>
            <span class="flex-grow"></span>
            <MenuIcon class="icon-2" :active="headerMenuExpanded" @click="menuIconClick" />
        </div>
        <nav v-if="headerMenuExpanded" 
            class="flex-stripe pad-05 flex-spacing-1 margin-bottom-05" 
            style="border-top: 2px solid var(--color-back-1)">
            <router-link to="/catalog/designs/list?page=0" class="top-navbar-link">Catalog</router-link>
            <template v-if="loggedIn">
                <router-link to="/profile" class="top-navbar-link">Profile</router-link>
            </template>
            <template v-else>
                <router-link to="/login" class="top-navbar-link">Log in</router-link>
                <router-link to="/signup" class="top-navbar-link">Sign up</router-link>
            </template>
            <span class="flex-grow"></span>
        </nav>
    </div>
</header>
</template>

<script>
import FwshIcon from "@/comp/icons/Fwsh.vue" 
import MenuIcon from "@/comp/icons/Menu.vue"

function data() {
    return {
        headerMenuExpanded: false,
        storage: this.$storage
    }
}

const loggedIn = {
    get() {
        return !!(this.storage.profile?.id)
    }
}

function menuIconClick() {
    this.headerMenuExpanded = !(this.headerMenuExpanded)
}

export default {
    data,
    computed: {
        loggedIn
    },
    methods: {
        menuIconClick
    },
    components: {
        FwshIcon,
        MenuIcon
    }
}
</script>