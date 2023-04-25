import index from "@/pages/index.vue"
import login from "@/pages/login.vue"
// import signup from "@/pages/signup.vue"
import profile from "@/pages/profile.vue"

import blueprintsIndex from "@/pages/blueprints/index.vue"
import designList from "@/pages/blueprints/designs/list.vue"
import designView from "@/pages/blueprints/designs/view.vue"
import designCreate from "@/pages/blueprints/designs/create.vue"

import peopleIndex from "@/pages/people/index.vue"
import customersList from "@/pages/people/customers/list.vue"
import suppliersList from "@/pages/people/suppliers/list.vue"
import workersList from "@/pages/people/workers/list.vue"

import ordersIndex from "@/pages/orders/index.vue"
import productionOrderList from "@/pages/orders/production/list.vue"
import productionOrderView from "@/pages/orders/production/view.vue"

// import catalog from "@/pages/catalog/index.vue"
// import designList from "@/pages/catalog/designs/list.vue" 
// import fabricList from "@/pages/catalog/fabrics/list.vue"
// import productionOrderList from "@/pages/orders/production/list.vue"
// import repairOrderList from "@/pages/orders/repair/list.vue"

import error404 from "@/pages/error404.vue" 

// Manager's Panel routes
//
const routes = [
    {
        path: "/",
        component: index
    },
    {
        path: "/index",
        component: index
    },
    {
        path: "/login",
        component: login
    },
    {
        path: "/profile",
        component: profile
    },
    {
        path: "/blueprints",
        component: blueprintsIndex
    },
    {
        path: "/blueprints/designs/list",
        component: designList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/blueprints/designs/view/:id",
        component: designView,
        props: ({ params }) => ({ id: Number(params.id) })
    },
    {
        path: "/blueprints/designs/create",
        component: designCreate
    },
    {
        path: "/people",
        component: peopleIndex
    },
    {
        path: "/people/customers/list",
        component: customersList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/people/workers/list",
        component: workersList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/people/suppliers/list",
        component: suppliersList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/orders",
        component: ordersIndex
    },
    {
        path: "/orders/production/:tab(list|archive)",
        component: productionOrderList,
        props: ({ params, query }) => ({ tab: params.tab, page: Number(query.page) })
    },
    {
        path: "/orders/production/view/:id",
        component: productionOrderView,
        props: ({ params }) => ({ id: Number(params.id) })
    },
    /*{
        path: "/catalog",
        component: catalog
    },
    {
        path: "/catalog/designs/list",
        component: designList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/catalog/fabrics/list",
        component: fabricList,
        props: ({ query }) => ({ page: Number(query.page) })
    },
    {
        path: "/orders/repair/:tab(list|archive)",
        component: repairOrderList,
        props: ({ params, query }) => ({ tab: params.tab, page: Number(query.page) })
    },*/
    {
        path: "/:garbage(.*)",
        component: error404,
        props: route => ({ path: route.params.garbage })
    }
]

export default routes
