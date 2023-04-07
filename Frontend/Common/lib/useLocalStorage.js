let existingStorages = { }

function getStorageObject (storageId) {
    try {
        return JSON.parse(window.localStorage[storageId])
    }
    catch (error) {
        return null
    }
}

function createLocalStorage (storageId, { initial, wrap, volatile, created } = options) {
    let storageObject = getStorageObject(storageId) ?? initial
    if (wrap instanceof Function) {
        storageObject = wrap(storageObject)
    } 
    if (created instanceof Function) {
        created.call(storageObject)
    }
    window.addEventListener("unload", () => {
        if (volatile instanceof Array) {
            for (const key of volatile) storageObject[key] = undefined 
        }
        window.localStorage[storageId] = JSON.stringify(storageObject)
    })
    return storageObject
} 

function useLocalStorage (storageId, options) {
    if (existingStorages[storageId] == undefined) {
        existingStorages[storageId] = createLocalStorage(storageId, options)
    }
    return existingStorages[storageId]
}

export { useLocalStorage }
