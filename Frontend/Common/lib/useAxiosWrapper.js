import { AxiosWrapper } from "./AxiosWrapper.js" 

const $cache = Symbol()

let existingWrappers = { }

function useAxiosWrapper (wrapperId, { baseUrl, cached }) {
    if (existingWrappers[wrapperId] == undefined) {
        let wrapper = AxiosWrapper({ id: wrapperId, baseUrl: baseUrl })
        if (cached === true) {
            // If cached, replace original get method with another method
            // that accepts cacheTTL option.
            // cacheTTL is measured in seconds.  
            wrapper._get = wrapper.get
            wrapper.get = function (options, extraOptions = null) {
                this[$cache] ??= { }
                let requestId = (options.baseUrl ?? extraOptions?.baseUrl ?? baseUrl) 
                            + (options.url ?? extraOptions?.url) 
                            + JSON.stringify(options.params)
                if (options.cacheTTL && this[$cache][requestId]) {
                    let cacheAge = Date.now() - this[$cache][requestId].timestamp
                    if (cacheAge < options.cacheTTL * 1000) {
                        console.log(`AxiosWrapper: using ${requestId} cached ${cacheAge}ms ago`)
                        return new Promise((resolve, reject) => resolve(this[$cache][requestId].response))
                    }
                }
                let promise = this._get(options, extraOptions)
                promise.then(response => {
                    if (response.status == 200) { 
                        let timestamp = Date.now()
                        this[$cache][requestId] = { timestamp, response }
                    }
                })
                return promise
            }
        }
        existingWrappers[wrapperId] = wrapper
    }
    return existingWrappers[wrapperId]
}

export { useAxiosWrapper }
