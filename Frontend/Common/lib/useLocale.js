let locales = { }

let locale = undefined

let onLocaleChangeCallbacks = [ ]

function selectLocale (localeKey, { fallbackToAnything } = { }) {
    locale = locales[localeKey] ?? (fallbackToAnything ? Object.values(locales)[0] : undefined)
    for (const callback of onLocaleChangeCallbacks) callback(locale)
}

function registerLocale (localeKey, locale) {
    locales[localeKey] = locale
}

function onLocaleChange (callback = null) {
    if (! (callback instanceof Function)) return
    if (onLocaleChangeCallbacks.find(f => f == callback)) return
    onLocaleChangeCallbacks.push(callback)
    callback(locale)
}

function useLocale () {
    return { locales, selectLocale, registerLocale, onLocaleChange }
}

export { useLocale }
