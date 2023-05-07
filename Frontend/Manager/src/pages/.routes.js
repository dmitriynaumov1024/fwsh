import { defineRoutes } from "@common"

import index from "./index.vue"
import login from "./login.vue"
import signup from "./signup.vue"
import profile from "./profile.vue"

export default defineRoutes ( "", [
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
        component: login,
        props: { next: String }
    },
    {
        path: "/signup",
        component: signup
    },
    {
        path: "/profile",
        component: profile
    }
])
