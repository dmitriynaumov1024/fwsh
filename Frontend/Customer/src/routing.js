import index from "@/pages/index.vue"
import login from "@/pages/login.vue"
import signup from "@/pages/signup.vue"
import profile from "@/pages/profile.vue"

import error404 from "@/pages/error404.vue" 

// Customer's Panel routes
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
        path: "/signup",
        component: signup
    },
    {
        path: "/profile",
        component: profile
    },
    {
        path: "/:garbage(.*)",
        component: error404,
        props: route => ({ path: route.params.garbage })
    }
]

export default routes
