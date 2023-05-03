import base from "@common/locales/en-GB.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Furniture workshop"
    },
    index: {
        title: "Furniture workshop",
    },
    header: {
        title: "workshop",
    }
})
