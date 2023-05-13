import { defineRoutes } from "@common"

import index from "./index.vue"
import designList from "./designs/list.vue"
import designView from "./designs/view.vue"
import fabricList from "./fabrics/list.vue"

export default defineRoutes("/catalog", [
    {
        path: "",
        component: index
    },
    {
        path: "/designs/list",
        component: designList,
        props: { page: Number, type: String, reverse: Boolean }
    },
    {
        path: "/designs/view/:id",
        component: designView,
        props: { id: Number }
    },
    {
        path: "/fabrics/list",
        component: fabricList,
        props: { page: Number, fabrictype: Number, color: Number, reverse: Boolean }
    }
])
