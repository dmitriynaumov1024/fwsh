import { defineRoutes } from "@common"

import index from "./index.vue"
import productionTaskList from "./production/list.vue"
import productionTaskView from "./production/view.vue"
import repairTaskList from "./repair/list.vue"
import repairTaskView from "./repair/view.vue"

export default defineRoutes ( "/tasks", [
    {
        path: "",
        component: index
    },
    {
        path: "/production/:tab(list|archive)",
        component: productionTaskList,
        props: { tab: String, page: Number, order: Number }
    },
    {
        path: "/production/view/:id",
        component: productionTaskView,
        props: { tab: String, id: Number }
    },
    {
        path: "/repair/:tab(list|archive)",
        component: repairTaskList,
        props: { tab: String, page: Number, order: Number }
    },
    {
        path: "/repair/view/:id",
        component: repairTaskView,
        props: { tab: String, id: Number }
    },
])
