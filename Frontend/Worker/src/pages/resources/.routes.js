import { defineRoutes } from "@common"

import index from "./index.vue"

import colorList from "./colors/list.vue"
import fabricTypeList from "./fabrictypes/list.vue"

import partList from "./parts/list.vue"
import materialList from "./materials/list.vue"
import fabricList from "./fabrics/list.vue"

export default defineRoutes ( "/resources", [
    {
        path: "",
        component: index
    },
    {
        path: "/colors/list",
        component: colorList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/fabrictypes/list",
        component: fabricTypeList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/parts/list",
        component: partList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/materials/list",
        component: materialList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/fabrics/list",
        component: fabricList,
        props: { page: Number, reverse: Boolean }
    }
])

