import { arrayConcat } from "@common/utils"

import routes from "./pages/.routes.js" 
import catalog from "./pages/catalog/.routes.js"
import orders from "./pages/orders/.routes.js"

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
    catalog,
    orders,
    garbage
)
