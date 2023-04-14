import index from "@/pages/index.vue"
import login from "@/pages/login.vue"
// import signup from "@/pages/signup.vue"
import profile from "@/pages/profile.vue"

// import catalog from "@/pages/catalog/index.vue"
// import designList from "@/pages/catalog/designs/list.vue" 
// import fabricList from "@/pages/catalog/fabrics/list.vue"
// import productionOrderList from "@/pages/orders/production/list.vue"
// import repairOrderList from "@/pages/orders/repair/list.vue"
// import productionOrderView from "@/pages/orders/production/view.vue"

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
        path: "/orders/production/:tab(list|archive)",
        component: productionOrderList,
        props: ({ params, query }) => ({ tab: params.tab, page: Number(query.page) })
    },
    {
        path: "/orders/production/view/:id",
        component: productionOrderView,
        props: ({ params }) => ({ id: Number(params.id) })
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
