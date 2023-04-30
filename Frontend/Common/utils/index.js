// convert something like ["key1", "key2"] 
// to { "key1": true, "key2": true }
//
function arrayToDict (array) {
    let result = { }
    if (array.constructor == Array) 
        for (const key of array) result[key] = true
    return result 
}

// Do content url resolution
//
function cdnResolve (url) {
    if (url?.startsWith && !url.startsWith("/")) url = "/" + url 
    return import.meta.env.VITE_CDN_BASEURL + "/files" + url
}

// Like Object.assign but nested
//
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

// Create a deep copy of object
//
function nestedObjectCopy (source) {
    return nestedObjectAssign({ }, source)
}

// Merge object structure, with variable number of arguments.
// None of sources is modified, and the merged result is returned
// 
function nestedObjectMerge (...sources) {
    let result = { }
    for (const source of sources) {
        nestedObjectAssign(result, source)
    }
    return result
}

export { 
    arrayToDict,
    cdnResolve,
    nestedObjectAssign,
    nestedObjectCopy,
    nestedObjectMerge,
}
