import { createApp } from "vue"
import { createRouter, createWebHistory } from "vue-router"

import App from "./main.vue"

import { AxiosWrapperPlugin, LocalStoragePlugin } from "Common"
import routes from "./routing.js"

let app = createApp(App)
app.config.unwrapInjectedRef = true

app.use(AxiosWrapperPlugin, "customer", "http://192.168.0.107:4000")
app.use(LocalStoragePlugin, "customer", { })
app.use(createRouter({
    history: createWebHistory(),
    routes: routes
}))

app.mount("#app-root")

import "Common/stylesheets"
