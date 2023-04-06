import { createRouter, createWebHistory } from "vue-router"
import routes from "./routing.js"

let router = createRouter({
    history: createWebHistory(),
    routes: routes
})

import { setupLocales } from "./locales.js"
setupLocales({ select: "en-GB" })

import { createApp, reactive, ref, provide } from "vue"
import App from "./main.vue"

let app = createApp(App)

app.use(router)

import { useAxiosWrapper, useLocalStorage, useLocale } from "Common"
let axios = useAxiosWrapper("customer", "http://192.168.0.107:4000")
let storage = useLocalStorage("customer", { }, reactive)
let { onLocaleChange } = useLocale()

let locale = ref(undefined)
onLocaleChange (newLocale => {
    locale.value = newLocale
})

app.provide("axios", axios)
app.provide("storage", storage)
app.provide("locale", locale)

app.mount("#app-root")

import "Common/stylesheets"
