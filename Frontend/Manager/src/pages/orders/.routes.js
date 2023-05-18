import { defineRoutes } from "@common"

import index from "./index.vue"

import productionOrderList from "./production/list.vue"
import productionOrderView from "./production/view.vue"

import repairOrderList from "./repair/list.vue"
import repairOrderView from "./repair/view.vue"

export default defineRoutes ( "/orders", [
    {
        path: "",
        component: index
    },
    {
        path: "/production/:tab(list|archive)",
        component: productionOrderList,
        props: { tab: String, page: Number }
    },
    {
        path: "/production/view/:id",
        component: productionOrderView,
        props: { tab: String, id: Number }
    },
    {
        path: "/repair/:tab(list|archive)",
        component: repairOrderList,
        props: { tab: String, page: Number }
    },
    {
        path: "/repair/view/:id",
        component: repairOrderView,
        props: { tab: String, id: Number }
    }
])
