import { defineRoutes } from "@common"

import index from "./index.vue"

import colorList from "./colors/list.vue"
import colorEdit from "./colors/edit.vue"

import fabricTypeList from "./fabrictypes/list.vue"

import partList from "./parts/list.vue"
import partEdit from "./parts/edit.vue"

import materialList from "./materials/list.vue"
import materialEdit from "./materials/edit.vue"

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
        path: "/colors/create",
        component: colorEdit
    },
    {
        path: "/colors/edit/:id",
        component: colorEdit,
        props: { id: Number }
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
        path: "/parts/create",
        component: partEdit
    },
    {
        path: "/parts/edit/:id",
        component: partEdit,
        props: { id: Number }
    },
    {
        path: "/materials/list",
        component: materialList,
        props: { page: Number, reverse: Boolean }
    },
    {
        path: "/materials/create",
        component: materialEdit
    },
    {
        path: "/materials/edit/:id",
        component: materialEdit,
        props: { id: Number }
    },
    {
        path: "/fabrics/list",
        component: fabricList,
        props: { page: Number, reverse: Boolean }
    }
])

