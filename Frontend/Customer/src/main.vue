<template>
<HeaderLayout />
<MainLayout />
<FooterLayout />
</template>

<script>
import HeaderLayout from "@/layout/Header.vue"
import MainLayout from "@/layout/Main.vue"
import FooterLayout from "@/layout/Footer.vue"

export default {
    components: {
        HeaderLayout,
        MainLayout,
        FooterLayout
    }
}
</script>
