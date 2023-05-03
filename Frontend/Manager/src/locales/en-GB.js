import base from "@common/locales/en-GB.js"

import { nestedObjectMerge } from "@common/utils"

export default nestedObjectMerge (base, {
    footer: {
        siteName: "Manager's panel"
    },
    index: {
        title: "Manager's panel",
    },
    header: {
        title: "workshop",
    }
})