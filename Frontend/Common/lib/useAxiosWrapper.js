import { AxiosWrapper } from "./AxiosWrapper.js" 

let existingWrappers = { }

function useAxiosWrapper (wrapperId, baseUrl) {
    if (existingWrappers[wrapperId] == undefined) {
        existingWrappers[wrapperId] = AxiosWrapper({ id: wrapperId, baseUrl: baseUrl })
    }
    return existingWrappers[wrapperId]
}

export { useAxiosWrapper }
