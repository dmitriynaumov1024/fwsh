import base from "./base.js"

import { nestedObjectMerge } from "../utils/index.js"

import { createDateFormat } from "../lib/createDateFormat.js"

const dateFormat = createDateFormat("en-GB")

export default nestedObjectMerge (base, {
    key: "en-GB",
    displayName: "English",
    dateFormat: dateFormat,
    yesNo: {
        [true]: "Yes",
        [false]: "No"
    },
    badFields: {
        before: "Please check",
        after: "once again."
    },
    action: {
        send: "Send",
        submit: "Submit",
        create: "Create",
        update: "Update",
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
        details: "Details",
        makeOrder: "Make order"
    },
    common: {
        id: "id",
        other: "Other",
        list: "List",
        archive: "Archive",
        page: "Page",
        pleaseTryAgain: "Please try again",
        somethingWrong: "Something went wrong",
        seeConsole: "See console for problem details", 
        createdAt: "Created at",
        daysSinceCreated: "Days since created",
        language: "language"
    },
    selectLanguage: {
        title: "Site language"
    },
    loading: {
        title: "Loading",
        description: "Please wait, loading may take some time. If this message does "+
            "not disappear after 10 seconds, please try reloading the page with CTRL+F5, "+
            "check your network connection, or contact site administrator."
    },
    unauthorized: {
        title: "Unauthorized",
        description: "You tried to access some resource that only allows authorized clients. "+
            "Please log in or sign up and then try again."
    },
    notFound: {
        title: "Bad request",
        description: "Please check parameters you've provided and try again."
    },
    notFound: {
        title: "Nothing here",
        description: "Sorry, data can not be loaded, or this page doesn't exist yet."
    },
    noDataYet: {
        title: "Nothing here yet",
        description: "Don't worry, this happens at the very beginning!"
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
        title: "workshop",
        nav: {
            catalog: "Catalog",
            profile: "Profile",
            login: "Log in",
            signup: "Sign up" 
        }
    },
    footer: {
        siteName: "fwsh"
    },
    index: {
        title: "fwsh",
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
    noSignup: {
        title: "Can not sign up",
        description: "Signup is not allowed at this moment. If you are sure this is an error, "+
            "please contact site administrator. If you already have account, please go to login page."
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
    notification: {
        single: "Notification",
        plural: "Notifications",
        new: "New notification"
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
    measureUnits: {
        unknown: "unknown",
        m: "m",
        m2: "sq.m",
        m3: "cub.m",
        L: "L",
        Kg: "Kg",
        g: "g"
    },
    resource: {
        single: "Resource",
        plural: "Resources",
        name: "Name",
        externalId: "External id",
        description: "Description",
        price: "Price",
        pricePer: "Price per",
        pricePerUnit: "Price per unit",
        measureUnit: "Measure unit",
        quantity: "Quantity",
        usedQuantity: "Used quantity",
        actualQuantity: "Actual quantity",
        inStock: "In stock",
        normalStock: "Normal stock",
        refillPeriodDays: "Refill period, days",
        lastCheckedAt: "Last checked at",
        lastRefilledAt: "Last refilled at",
        needsRefill: "Critical amount, needs refill",
        isTimeToRefill: "It's time to refill",
        createdAt: "Created at",
    },
    part: {
        single: "Part",
        plural: "Parts",
    },
    material: {
        single: "Material",
        plural: "Materials",
    },
    fabric: {
        catalog: "Fabric catalog",
        single: "Fabric",
        plural: "Fabrics"
    },
    fabricType: {
        single: "Fabric type",
        plural: "Fabric types",
        name: "Name",
        description: "Description"
    },
    color: {
        single: "Color",
        plural: "Colors",
        name: "Name",
        rgbCode: "RGB code"
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
    furnitureTypes: {
        unknown: "Unknown",
        ottoman: "Ottoman",
        sofa: "Sofa",
        corner: "Corner",
        armchair: "Armchair",
        pouffe: "Pouffe"
    },
    taskPrototype: {
        single: "Task prototype",
        plural: "Task prototypes",
    },
    task: {
        single: "Task",
        plural: "Tasks",
        status: "Status",
        createdAt: "Created at",
        startedAt: "Work started at",
        finishedAt: "Finished at",
        workType: "Work type",
        role: "Role",
        resourceUsage: "Resource usage",
        instructions: "Instructions",
    },
    productionTask: {
        single: "Production task",
        plural: "Production tasks",
    },
    repairTask: {
        single: "Repair task",
        plural: "Repair tasks",
    }
})
