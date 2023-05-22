import { defineRoutes } from "@common"

import paycheckList from "./list.vue"

export default defineRoutes ( "/paychecks", [
    {
        path: "/:tab(list|archive)",
        component: paycheckList,
        props: { tab: String, page: Number }
    }
])
