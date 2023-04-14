import { createDateFormat } from "@common"

const dateFormat = createDateFormat("en-GB")

export default {
    key: "en-GB",
    displayName: "English",
    yesNo: {
        [true]: "Yes",
        [false]: "No"
    },
    action: {
        edit: "Edit",
        save: "Save",
        confirm: "Confirm",
        cancel: "Cancel",
        delete: "Delete",
        signup: "Sign up",
        login: "Log in",
        logout: "Log out",
        details: "Details"
    },
    common: {
        catalog: "Catalog",
        list: "List",
        archive: "Archive",
        page: "Page",
        pleaseTryAgain: "Please try again",
        somethingWrong: "Something went wrong",
        seeConsole: "See console for problem details", 
        loading: "Loading! Please wait...",
    },
    header: {
        nav: {
            catalog: "Catalog",
            profile: "Profile",
            login: "Log in",
            signup: "Sign up" 
        }
    },
    login: {
        welcomeBack: "Welcome back!",
        haveNoAccount: "Have no account yet?"
    },
    signup: {
        title: "Create account",
        success: "Success!",
        successDescription: "Your account was created. Now you can log in and get the job done!",
        goToLogin: "Go to login",
        iAmOrganization: "I am representing organization",
        alreadyHaveAccount: "Already have account?"
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
    photo: {
        single: "Photo",
        plural: "Photos"
    },
    status: {
        unknown: "Unknown",
        submitted: "Submitted",
        assigned: "Assigned",
        delayed: "Delayed",
        working: "Working",
        finished: "Finished",
        impossible: "Impossible",
        rejected: "Rejected",
        receivedpaid: "Received and paid"
    },
    order: {
        single: "Order",
        plural: "Orders",
        status: "Status",
        customer: "Customer",
        createdAt: "Created at",
        startedAt: "Work started at",
        finishedAt: "Finished at",
        receivedAt: "Received at",
        notifications: "Notifications"
    },
    productionOrder: {
        single: "Production order",
        plural: "Production orders",
        details: "Order details",
        quantity: "Quantity",
        pricePerOne: "Price per one",
        priceTotal: "Price total",
        design: "Design",
        fabric: "Fabric",
        decorMaterial: "Decor. material"
    },
    repairOrder: {
        single: "Repair order",
        plural: "Repair orders",
        price: "Price",
        prepayment: "Prepayment",
        description: "Description"
    },
    fabric: {
        catalog: "Fabric catalog",
        single: "Fabric",
        plural: "Fabrics"
    },
    design: {
        catalog: "Design catalog",
        single: "Design",
        plural: "Designs",
        displayName: "Name",
        type: "Type",
        price: "Price",
        isTransformable: "Transformable",
        dimCompact: "Compact dimensions (WxLxH)",
        dimExpanded: "Expanded dimensions (WxLxH)",
        createdAt: "Created at"
    },
    formatBadFields (badFields, selector) {
        if (!badFields instanceof Array) badFields = Object.keys(badFields)
        if (selector instanceof Function) 
            badFields = badFields.map(key => selector(this)[key]).filter(key => key != undefined)
        return "Please check " + badFields.map(field => field.toLowerCase()).join(", ") + " once again."
    },
    formatDate (date) {
        date = new Date(date)
        return dateFormat.format(date)
    },
    formatDateTime (date) {
        date = new Date(date)
        return dateFormat.format(date) + " " + date.toLocaleTimeString(this.key)
    }
}
