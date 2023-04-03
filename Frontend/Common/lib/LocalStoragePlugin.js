export const LocalStoragePlugin = {
    install (app, storageId, defaultValue) {
        let storageObject = undefined 
        try {
            storageObject = JSON.parse(window.localStorage[storageId])
        }
        catch (error) {
            storageObject = defaultValue
        }
        app.config.globalProperties.$storage = storageObject
        window.addEventListener("unload", () => {
            window.localStorage[storageId] = JSON.stringify(storageObject)
        })
    }
}
