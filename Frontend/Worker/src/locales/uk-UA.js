import base from "@common/locales/uk-UA.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Worker's panel"
    },
    index: {
        title: "Панель робітника",
    },
    header: {
        title: "workshop",
    }
})
