// convert something like ["key1", "key2"] 
// to { "key1": true, "key2": true }
//
function arrayToDict (array) {
    let result = { }
    if (array.constructor == Array) 
        for (const key of array) result[key] = true
    return result 
}

function cdnResolve (url) {
    if (url?.startsWith && !url.startsWith("/")) url = "/" + url 
    return import.meta.env.VITE_CDN_BASEURL + "/files" + url
}

function nestedObjectMerge (...sources) {
    let result = { }
    for (const source of sources) {
        nestedObjectAssign(result, source)
    }
    return result
}

function nestedObjectAssign (target, patch) {
    if (patch instanceof Object) for (const key in patch) {
        let propValue = patch[key]
        if (propValue instanceof Object && !(propValue instanceof Function)) {
            if (!(target[key] instanceof Object)) target[key] = { }
            nestedObjectAssign(target[key], propValue)
        }
        else {
            target[key] = propValue
        }
    }
    return target
}

export { 
    arrayToDict,
    cdnResolve,
    nestedObjectMerge,
}
