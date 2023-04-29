<template>
<header class="header">
    <div class="width-container">
        <div class="flex-stripe pad-05">
            <FwshIcon class="icon-2" />
            <router-link to="/" class="link"><h2>{{locale.header.title}}</h2></router-link>
            <span class="flex-grow"></span>
            <ToggleButton v-model="headerMenuExpanded" v-slot="{ active }">
                <XIcon v-if="active" class="icon-2" />
                <MenuIcon v-else class="icon-2" />
            </ToggleButton>
        </div>
        <nav v-if="headerMenuExpanded" 
            class="flex-stripe pad-05 flex-spacing-1 margin-bottom-05" 
            style="border-top: var(--border)">
            <slot name="nav"></slot>
            <span class="flex-grow"></span>
        </nav>
    </div>
</header>
</template>

<script setup>
import { ref, inject } from "vue"
import ToggleButton from "../comp/ctrl/ToggleButton.vue"
import FwshIcon from "../comp/icons/Fwsh.vue" 
import MenuIcon from "../comp/icons/Menu.vue"
import XIcon from "../comp/icons/X.vue"

let headerMenuExpanded = ref(true)

const locale = inject("locale")

</script>
