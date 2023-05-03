import base from "@common/locales/en-GB.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Worker's panel"
    },
    index: {
        title: "Worker's panel",
    },
    header: {
        title: "workshop",
    }
})
