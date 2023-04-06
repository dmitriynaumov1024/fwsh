let existingStorages = { }

function getStorageObject (storageId) {
    try {
        return JSON.parse(window.localStorage[storageId])
    }
    catch (error) {
        return null
    }
}

function createLocalStorage (storageId, initialValue, wrap = null) {
    let storageObject = getStorageObject(storageId) ?? initialValue
    if (wrap instanceof Function) storageObject = wrap(storageObject) 
    window.addEventListener("unload", () => {
        window.localStorage[storageId] = JSON.stringify(storageObject)
    })
    return storageObject
} 

function useLocalStorage (storageId, initialValue, wrap = null) {
    if (existingStorages[storageId] == undefined) {
        existingStorages[storageId] = createLocalStorage(storageId, initialValue, wrap)
    }
    return existingStorages[storageId]
}

export { useLocalStorage }
