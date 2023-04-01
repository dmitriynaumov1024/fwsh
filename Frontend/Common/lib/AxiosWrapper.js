import axios from "axios"

function AxiosWrapper ({ id = "shared", baseUrl } = options) {
    
    let auth = window.localStorage[`AxiosWrapper-${id}-auth`] ?? ""

    let runAxiosRequest = (method, options, extraOptions) => {
        let promise = axios ({
            method: method,
            url: options?.url ?? extraOptions?.url,
            baseURL: options?.baseUrl ?? extraOptions?.baseUrl ?? baseUrl,
            headers: { 
                "Authorization": auth, 
                "Content-Type": options?.contentType ?? extraOptions?.contentType ?? "application/json"
            },
            params: options?.params,
            data: options?.data,
            validateStatus: options?.validateStatus ?? extraOptions?.validateStatus ?? (() => true)
        })
        promise.then(response => {
            auth = response.headers["authorization"] ?? auth
            window.localStorage[`AxiosWrapper-${id}-auth`] = auth
        })
        return promise
    }

    return {
        "get" (options, extraOptions = null) {
            return runAxiosRequest("get", options, extraOptions)
        },
        "post" (options, extraOptions = null) {
            return runAxiosRequest("post", options, extraOptions)
        },
        "put" (options, extraOptions = null) {
            return runAxiosRequest("put", options, extraOptions)
        },
        "delete" (options, extraOptions = null) {
            return runAxiosRequest("delete", options, extraOptions)
        },
        "ping" (url, { expectedTrueStatus = true, expectedFalseStatus = undefined } = options) {
            return new Promise((resolve, reject) => {
                runAxiosRequest("get", { url }, null)
                .then(response => {
                    if (response.status === expectedFalseStatus) resolve(false)
                    if (response.status === expectedTrueStatus || expectedTrueStatus === true) resolve(true)
                })
                .catch(error => {
                    reject(error)
                })
            })
        }
    }
}

export default AxiosWrapper
