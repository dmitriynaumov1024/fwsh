import base from "@common/locales/uk-UA.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Furniture workshop"
    },
    index: {
        title: "Меблева майстерня",
    },
    header: {
        title: "workshop",
    }
})
