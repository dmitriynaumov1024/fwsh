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
        submit: "Submit",
        create: "Create",
        addPhotos: "Add photos",
        reset: "Reset",
        clear: "Clear",
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
        id: "id",
        list: "List",
        archive: "Archive",
        page: "Page",
        pleaseTryAgain: "Please try again",
        somethingWrong: "Something went wrong",
        seeConsole: "See console for problem details", 
    },
    noData: {
        title: "Nothing here",
        description: "Sorry, data can not be loaded, or this page doesn't exist yet."
    },
    noRoute: {
        title: "Error 404",
        description: "Seems like you tried to navigate to non-existent page "
    },
    header: {
        nav: {
            catalog: "Catalog",
            profile: "Profile",
            login: "Log in",
            signup: "Sign up" 
        }
    },
    index: {
        title: "Manager's panel - Main page",
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
    roles: {
        unknown: "Unknown",
        customer: "Customer",
        worker: "Worker",
        management: "Management",
        carpentry: "Carpentry",
        sewing: "Sewing",
        assembly: "Assembly",
        upholstery: "Upholstery"
    },
    person: {
        single: "Person",
        plural: "People",
    },
    customer: {
        single: "Customer",
        plural: "Customers",
    },
    manager: {
        single: "Manager",
        plural: "Managers",
    },
    worker: {
        single: "Worker",
        plural: "Workers",
    },
    supplier: {
        single: "Supplier",
        plural: "Suppliers",
    },
    profile: {
        myProfile: "My profile",
        myProductionOrders: "My production orders",
        myRepairOrders: "My repair orders",
        surname: "Surname",
        name: "Name",
        patronym: "Patronym",
        phone: "Phone number",
        email: "E-mail",
        orgName: "Org. name",
        roles: "Role(s)",
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
        quantity: "Quantity",
        pricePerOne: "Price per one",
        priceTotal: "Price total",
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
    blueprint: {
        single:"Blueprint",
        plural: "Blueprints",
    },
    design: {
        notFound: "Can not find design with given id or name key",
        creationMessage: "Successfully created new design!",
        create: "Create design",
        list: "Design list",
        catalog: "Design catalog",
        single: "Design",
        plural: "Designs",
        nameKey: "Name key",
        displayName: "Name",
        type: "Type",
        price: "Price",
        description: "Description",
        isVisible: "Visible in catalog",
        isTransformable: "Transformable",
        dimCompact: "Compact dimensions (WxLxH)",
        dimExpanded: "Expanded dimensions (WxLxH)",
        length: "Length",
        width: "Width",
        height: "Height",
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
