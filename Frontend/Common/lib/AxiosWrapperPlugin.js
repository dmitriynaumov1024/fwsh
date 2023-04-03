import { AxiosWrapper } from "./AxiosWrapper.js"

export const AxiosWrapperPlugin = {
    install (app, wrapperId, baseUrl) {
        app.config.globalProperties.$axios = AxiosWrapper({ id: wrapperId, baseUrl: baseUrl })
    }
}
