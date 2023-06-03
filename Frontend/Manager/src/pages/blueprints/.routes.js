import { defineRoutes } from "@common"

import index from "./index.vue"

import designList from "./designs/list.vue"
import designView from "./designs/view.vue"
import designCreate from "./designs/create.vue"
import designEdit from "./designs/edit.vue"

import taskPrototypeList from "./taskprototypes/list.vue"
import taskPrototypeEdit from "./taskprototypes/edit.vue"

export default defineRoutes ( "/blueprints", [
    {
        path: "",
        component: index,
    },
    {
        path: "/designs/list",
        component: designList,
        props: { page: Number }
    },
    {
        path: "/designs/view/:id",
        component: designView,
        props: { id: Number }
    },
    {
        path: "/designs/edit/:id",
        component: designEdit,
        props: { id: Number }
    },
    {
        path: "/designs/create",
        component: designEdit
    },
    {
        path: "/taskprototypes/list",
        component: taskPrototypeList,
        props: { design: Number }
    },
    {
        path: "/taskprototypes/create",
        component: taskPrototypeEdit,
        props: { design: Number }
    },
    {
        path: "/taskprototypes/edit/:id",
        component: taskPrototypeEdit,
        props: { id: Number }
    }
])
