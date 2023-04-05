import { createDateFormat } from "Common"

dateFormat = createDateFormat("en-GB")

export default {
    key: "en-GB",
    diplayName: "English",
    header: {
        nav: {
            profile: "Profile",
            login: "Log in",
            signup: "Sign up" 
        }
    },
    formatDate(date) {
        date = new Date(date)
        return dateFormat.format(date)
    }
}
