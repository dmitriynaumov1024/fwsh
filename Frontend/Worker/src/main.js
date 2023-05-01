import { createApp, reactive, ref, provide } from "vue"
import App from "@/layout/App.vue"

let app = createApp(App)

import { createRouter, createWebHistory } from "vue-router"
import routes from "@/routing.js"

let router = createRouter({
    history: createWebHistory(),
    routes: routes
})

app.use(router)

import { useAxiosWrapper, useLocalStorage, useLocale } from "@common"

let axios = useAxiosWrapper("worker", { 
    baseUrl: import.meta.env.VITE_API_BASEURL,
    cached: true
})

let storage = useLocalStorage("worker", { 
    initial: { }, 
    wrap: reactive, 
    volatile: [ "tmp" ],
    created() {
        this.tmp = { }
        axios.get({
            url: "/worker/profile/view"
        })
        .then(({ status, data: response } = axiosresponse) => {
            if (status < 299 && response.id) {
                this.tmp.profile = response
            }
        })
    }
})

import { setupLocales } from "@/locales.js"
setupLocales()

let { selectLocale, onLocaleChange } = useLocale()
selectLocale(storage.localeKey ?? "en-GB", { fallbackToAnything: true })
let locale = ref(undefined)
onLocaleChange (newLocale => {
    locale.value = newLocale
    storage.localeKey = newLocale.key
})

app.provide("locale", locale)
app.provide("axios", axios)
app.provide("storage", storage)

app.mount("#app-root")

import "@common/stylesheets"
