import { defineRoutes } from "@common"

import index from "./index.vue"

import designList from "./designs/list.vue"
import designView from "./designs/view.vue"
import designCreate from "./designs/create.vue"

export default defineRoutes ( "/blueprints", [
    {
        path: "",
        component: index,
    },
    {
        path: "/designs/list",
        component: designList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/designs/view/:id",
        component: designView,
        props: { id: Number }
    },
    {
        path: "/designs/create",
        component: designCreate
    },
])
