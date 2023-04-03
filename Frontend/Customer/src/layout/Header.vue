<template>
<header class="header">
    <div class="width-container">
        <div class="flex-stripe pad-05">
            <router-link to="/" class="link"><h1>Furniture workshop</h1></router-link>
            <span class="flex-grow"></span>
            <MenuIcon class="icon-2" :active="headerMenuExpanded" @click="menuIconClick" />
        </div>
        <div v-if="headerMenuExpanded" class="flex-stripe pad-h-05 flex-spacing-1 margin-bottom-1">
            <router-link to="/catalog/designs/list?page=0" class="link">Catalog</router-link>
            <template v-if="loggedIn">
                <router-link to="/profile" class="link">Profile</router-link>
            </template>
            <template v-else>
                <router-link to="/login" class="link">Log in</router-link>
                <router-link to="/signup" class="link">Sign up</router-link>
            </template>
            <span class="flex-grow"></span>
        </div>
    </div>
</header>
</template>

<script>
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
        MenuIcon
    }
}
</script>