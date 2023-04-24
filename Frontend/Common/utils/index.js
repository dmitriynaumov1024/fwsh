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

export { 
    arrayToDict,
    cdnResolve,
}
