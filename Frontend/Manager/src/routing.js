import { arrayConcat } from "@common/utils"

import routes from "./pages/.routes.js" 
import blueprints from "./pages/blueprints/.routes.js"
import orders from "./pages/orders/.routes.js"
import people from "./pages/people/.routes.js"
import resources from "./pages/resources/.routes.js"

import error404 from "./pages/error404.vue"

const garbage = [
    {
        path: "/:garbage(.*)",
        component: error404,
        props: route => ({ path: route.params.garbage })
    }
]

export default arrayConcat (
    routes,
    blueprints,
    orders,
    people,
    resources,
    garbage
)
