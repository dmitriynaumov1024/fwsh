import AxiosWrapper from "./AxiosWrapper.js"

export default {
    install(app, wrapperId, baseUrl) {
        app.config.globalProperties.$axios = AxiosWrapper({ id: wrapperId, baseUrl: baseUrl })
    }
}
