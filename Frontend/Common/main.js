// all .css files will be minified and concatenated into dist/style.css
//
import "./css/base.css"
import "./css/colors.css" 
import "./css/containers.css"
import "./css/buttons.css"
import "./css/inputs.css"
import "./css/text.css"
import "./css/main.css"

import AxiosWrapper from "./lib/AxiosWrapper.js"
import AxiosWrapperPlugin from "./lib/AxiosWrapperPlugin.js"

export {
    AxiosWrapper,
    AxiosWrapperPlugin
}
