// all .css files will be minified and concatenated into dist/style.css
//
import "./css/base.css"
import "./css/colors.css" 
import "./css/containers.css"
import "./css/buttons.css"
import "./css/inputs.css"
import "./css/tables.css"
import "./css/icons.css"
import "./css/text.css"
import "./css/main.css"

export { createDateFormat } from "./lib/createDateFormat.js"

export { useAxiosWrapper } from "./lib/useAxiosWrapper.js"
export { useLocalStorage } from "./lib/useLocalStorage.js"
export { useLocale } from "./lib/useLocale.js"

export { arrayToDict } from "./lib/utils.js"

// I guess I have no other option
