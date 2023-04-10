import { useLocale } from "@common"
import en_GB from "@/locales/en-GB.js"
import uk_UA from "@/locales/uk-UA.js"

const availableLocales = [
    en_GB,
    uk_UA
]

function setupLocales ({ select }) {
    const { locales, registerLocale, selectLocale } = useLocale()
    for (const locale of availableLocales) {
        registerLocale(locale.key, locale)
    }
    if (select === true) {
        selectLocale(availableLocales[0].key)
    }
    else if (select) {
        selectLocale(select) 
    }
}

export { setupLocales }
