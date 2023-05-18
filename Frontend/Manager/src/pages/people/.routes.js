import { defineRoutes } from "@common"

import index from "./index.vue"

import customerList from "./customers/list.vue"
import workerList from "./workers/list.vue"
import supplierList from "./suppliers/list.vue"
import supplierEdit from "./suppliers/edit.vue"

export default defineRoutes ( "/people", [
    {
        path: "",
        component: index,
    },
    {
        path: "/customers/list",
        component: customerList,
        props: { page: Number }
    },
    {
        path: "/workers/list",
        component: workerList,
        props: { page: Number }
    },
    {
        path: "/suppliers/list",
        component: supplierList,
        props: { page: Number }
    },
    {
        path: "/suppliers/edit/:id",
        component: supplierEdit,
        props: { id: Number }
    },
    {
        path: "/suppliers/create",
        component: supplierEdit
    },
])
