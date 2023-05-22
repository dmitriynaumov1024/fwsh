import { arrayConcat } from "@common/utils"

import routes from "./pages/.routes.js"
import resources from "./pages/resources/.routes.js"
import tasks from "./pages/tasks/.routes.js"
import paychecks from "./pages/paychecks/.routes.js"

import error404 from "./pages/error404.vue"

const garbage = [{
    path: "/:garbage(.*)",
    component: error404,
    props: route => ({ path: route.params.garbage })
}]

export default arrayConcat (
    routes,
    resources,
    tasks,
    paychecks,
    garbage
)
