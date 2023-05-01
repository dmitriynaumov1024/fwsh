import index from "@/pages/index.vue"

import login from "@/pages/login.vue"
import signup from "@/pages/signup.vue"
import profile from "@/pages/profile.vue"

// import blueprintsIndex from "@/pages/blueprints/index.vue"
// import designList from "@/pages/blueprints/designs/list.vue"
// import designView from "@/pages/blueprints/designs/view.vue"
// import designCreate from "@/pages/blueprints/designs/create.vue"

// import ordersIndex from "@/pages/orders/index.vue"
// import productionOrderList from "@/pages/orders/production/list.vue"
// import productionOrderView from "@/pages/orders/production/view.vue"

import resourcesIndex from "@/pages/resources/index.vue"
import colorList from "@/pages/resources/colors/list.vue"
import fabricTypeList from "@/pages/resources/fabrictypes/list.vue"
import partList from "@/pages/resources/parts/list.vue"
import materialList from "@/pages/resources/materials/list.vue"
import fabricList from "@/pages/resources/fabrics/list.vue"

import error404 from "@/pages/error404.vue" 

// Worker's Panel routes
//
const routes = [
    {
        path: "/",
        component: index
    },
    {
        path: "/login",
        component: login
    },
    {
        path: "/signup",
        component: signup
    },
    {
        path: "/profile",
        component: profile
    },
    {
        path: "/resources",
        component: resourcesIndex
    },
    {
        path: "/resources/colors/list",
        component: colorList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/resources/fabrictypes/list",
        component: fabricTypeList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/resources/parts/list",
        component: partList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/resources/materials/list",
        component: materialList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/resources/fabrics/list",
        component: fabricList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/:garbage(.*)",
        component: error404,
        props: route => ({ path: route.params.garbage })
    }
]

export default routes
