import { createDateFormat } from "Common"

const dateFormat = createDateFormat("en-GB")

export default {
    key: "en-GB",
    displayName: "English",
    action: {
        edit: "Edit",
        save: "Save",
        confirm: "Confirm",
        cancel: "Cancel",
        delete: "Delete",
        logout: "Log out",
        details: "Details"
    },
    header: {
        nav: {
            catalog: "Catalog",
            profile: "Profile",
            login: "Log in",
            signup: "Sign up" 
        }
    },
    profile: {
        myProfile: "My profile",
        myProductionOrders: "My production orders",
        myRepairOrders: "My repair orders",
        surname: "Surname",
        name: "Name",
        patronym: "Patronym",
        phone: "Phone",
        email: "E-mail",
        orgName: "Org. name",
        discount: "Discount",
        createdAt: "Signup date",
        password: "Password",
        newPassword: "New password"
    },
    formatDate(date) {
        date = new Date(date)
        return dateFormat.format(date)
    }
}
