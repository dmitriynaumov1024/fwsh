import { createApp, reactive, ref, provide } from "vue"
import App from "@/layout/App.vue"

let app = createApp(App)

import { createRouter, createWebHistory } from "vue-router"
import routes from "./routing.js"

let router = createRouter({
    history: createWebHistory(),
    routes: routes
})

app.use(router)

import { setupLocales } from "./locales.js"
setupLocales({ select: "en-GB" })

import { useAxiosWrapper, useLocalStorage, useLocale } from "@common"

let axios = useAxiosWrapper("customer", "http://192.168.0.107:4000")
let storage = useLocalStorage("customer", { 
    initial: { }, 
    wrap: reactive, 
    volatile: [ "tmp" ],
    created() {
        this.tmp = { }
    }
})

let { onLocaleChange } = useLocale()
let locale = ref(undefined)
onLocaleChange (newLocale => {
    locale.value = newLocale
})

app.provide("locale", locale)
app.provide("axios", axios)
app.provide("storage", storage)

app.mount("#app-root")

import "@common/stylesheets"
