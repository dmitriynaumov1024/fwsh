import { createApp } from "vue"

import App from "./main.vue"

import { AxiosWrapperPlugin } from "Common"

let app = createApp(App);
app.config.unwrapInjectedRef = true

app.use(AxiosWrapperPlugin, "customer", "http://localhost:4000")

app.mount("#app-root")

import "Common/stylesheets"
