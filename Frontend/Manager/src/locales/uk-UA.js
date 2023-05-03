import base from "@common/locales/uk-UA.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Manager's panel"
    },
    index: {
        title: "Панель менеджера",
    },
    header: {
        title: "workshop",
    }
})
