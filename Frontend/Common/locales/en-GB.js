import base from "./base.js"

import { nestedObjectMerge } from "../utils/index.js"

import { createDateFormat } from "../lib/createDateFormat.js"

const dateFormat = createDateFormat("en-GB")

export default nestedObjectMerge (base, {
    key: "en-GB",
    displayName: "English",
    dateFormat: {
        format: dateFormat.format
    },
    yesNo: {
        [true]: "Yes",
        [false]: "No"
    },
    badFields: {
        before: "Please check",
        after: "once again."
    },
    action: {
        back: "Back",
        next: "Next",
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
        makeOrder: "Make order",
        assignWorker: "Assign worker", 
    },
    common: {
        id: "id",
        other: "Other",
        list: "List",
        archive: "Archive",
        catalog: "Catalog",
        page: "Page",
        pleaseTryAgain: "Please try again",
        somethingWrong: "Something went wrong",
        seeConsole: "See console for problem details", 
        createdAt: "Created at",
        daysSinceCreated: "Days since created",
        language: "language",
        notSelected: "Not selected",
        notAssigned: "Not assigned",
    },
    setVisible: {
        [true]: "Hide",
        [false]: "Show",
    },
    setVisibleResult: {
        [false]: "Hidden from users",
        [true]: "Visible for users"    
    },
    setActive: {
        [true]: "Deactivate",
        [false]: "Activate"
    },
    setActiveResult: {
        [true]: "Activated",
        [false]: "Deactivated"
    },
    selectLanguage: {
        title: "Site language"
    },
    changesSaved: {
        title: "Success",
        description: "All changes were successfully saved."
    },
    loading: {
        title: "Loading",
        description: "Please wait, loading may take some time. If this message does "+
            "not disappear after 10 seconds, please try reloading the page with CTRL+F5, "+
            "check your network connection, or contact site administrator."
    },
    badRequest: {
        title: "Bad request",
        description: "Please make sure query parameters match all requirements and try again."
    },
    unauthorized: {
        title: "Unauthorized",
        description: "You tried to access some resource that only allows authorized clients. "+
            "Please log in or sign up and then try again."
    },
    notFound: {
        title: "Nothing here",
        description: "Sorry, data can not be loaded, or this page doesn't exist yet.",
        before: "Can not find",
        after: ""
    },
    alreadyExists: {
        before: "",
        after: "already exists"
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
    error: {
        title: "Unknown error",
        description: "Something went wrong. See console for details."
    },
    saveFailed: {
        title: "Could not save changes",
        description: "Could not save changes most likely because of server fault. Check console for problem details."
    },
    tryLater: {
        title: "Too early",
        description: "The requested functionality is not available yet. Please try again later.",
        before: "Please try later at",
    },
    photoLimitExceeded: {
        title: "Photo limit exceeded",
        description: "Photo limit was exceeded.",
        getDescription(n) {
            return `Photo limit (${n}) was exceeded.`
        }
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
        create: "Create new customer"
    },
    manager: {
        single: "Manager",
        plural: "Managers",
        create: "Create new manager"
    },
    worker: {
        single: "Worker",
        plural: "Workers",
        create: "Create new worker"
    },
    supplier: {
        single: "Supplier",
        plural: "Suppliers",
        create: "Create new supplier"
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
        isOrganization: "Is organization",
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
        received: "Received",
        paid: "Paid",
        receivedpaid: "Received and paid"
    },
    notification: {
        single: "Notification",
        plural: "Notifications",
        new: "New notification",
        readAll: "Mark all as read",
    },
    myOrder: {
        single: "My order",
        plural: "My orders"
    },
    myTask: {
        single: "My task",
        plural: "My tasks"
    },
    order: {
        single: "Order",
        plural: "Orders",
        status: "Status",
        isActive: "Active",
        price: "Price",
        payment: "Payment",
        customer: "Customer",
        createdAt: "Created at",
        startedAt: "Work started at",
        finishedAt: "Finished at",
        receivedAt: "Received at",
        notifications: "Notifications",
        details: "Order details"
    },
    productionOrder: {
        single: "Production order",
        plural: "Production orders",
        create: "New production order",
        quantity: "Quantity",
        pricePerOne: "Price per one",
        priceTotal: "Price total",
        design: "Design",
        fabric: "Fabric",
        decor: "Decor. material"
    },
    repairOrder: {
        single: "Repair order",
        plural: "Repair orders",
        create: "New repair order",
        price: "Price",
        prepayment: "Prepayment",
        description: "Description"
    },
    measureUnits: {
        [null]: "unknown",
        unknown: "unknown",
        mm: "mm",
        cm: "cm",
        m: "m",
        m2: "sq.m",
        m3: "cub.m",
        L: "L",
        Kg: "Kg",
        g: "g"
    },
    slotNames: {
        [null]: "Unknown",
        decor: "Decor",
        fabric: "Fabric",
    },
    resource: {
        single: "Resource",
        plural: "Resources",
        type: "Type",
        slotName: "Slot",
        name: "Name",
        externalId: "External id",
        description: "Description",
        price: "Price",
        pricePer: "Price per",
        pricePerUnit: "Price per unit",
        measureUnit: "Measure unit",
        precision: "Precision, decimals",
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
        hasSupplyOrder: "Has active supply order",
        createdAt: "Created at",
    },
    resourceQuantity: {
        single: "Resource quantity",
        plural: "Resource quantities",
        expectQuantity: ""
    },
    resourceUsage: {
        single: "Resource usage",
        plural: "Resource usage",
        expectQuantity: "Expected quantity",
        actualQuantity: "Actual quantity"
    },
    part: {
        single: "Part",
        plural: "Parts",
        create: "Create new part"
    },
    material: {
        single: "Material",
        plural: "Materials",
        create: "Create new material"
    },
    decor: {
        single: "Decor material",
        plural: "Decor materials",
        create: "Create new decor material"
    },
    fabric: {
        catalog: "Fabric catalog",
        single: "Fabric",
        plural: "Fabrics",
        create: "Create new fabric"
    },
    fabricType: {
        single: "Fabric type",
        plural: "Fabric types",
        create: "Create new fabric type",
        name: "Name",
        description: "Description"
    },
    color: {
        single: "Color",
        plural: "Colors",
        create: "Create new color",
        edit: "Edit existing color",
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
        priceStartsAt: "starting at",
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
        [null]: "Not specified",
        unknown: "Unknown",
        ottoman: "Ottoman",
        minisofa: "Minisofa",
        sofa: "Sofa",
        corner: "Corner",
        armchair: "Armchair",
        pouffe: "Pouffe"
    },
    taskPrototype: {
        single: "Task prototype",
        plural: "Task prototypes",
        description: "Description",
        precedence: "Precedence"
    },
    task: {
        single: "Task",
        plural: "Tasks",
        status: "Status",
        isActive: "Active",
        createdAt: "Created at",
        startedAt: "Work started at",
        finishedAt: "Finished at",
        workType: "Work type",
        role: "Role",
        payment: "Payment",
        description: "Description",
        resourceUsage: "Resource usage",
        instructions: "Instructions",
        notAssigned: "Not assigned"
    },
    productionTask: {
        single: "Production task",
        plural: "Production tasks",
    },
    repairTask: {
        single: "Repair task",
        plural: "Repair tasks",
    },
    furniture: {
        single: "Furniture",
        plural: "Furnitures"
    },
    supplyOrder: {
        single: "Supply order",
        plural: "Supply orders",
        externalId: "External id",
        expectPrice: "Expected price",
        expectPricePerUnit: "Expected price",
        expectQuantity: "Expected quantity",
        actualPrice: "Actual price",
        actualPricePerUnit: "Actual price",
        actualQuantity: "Actual quantity",
        submitOrder: "Submit order",
        closeOrder: "Close order"
    },
    paycheck: {
        single: "Paycheck",
        plural: "Paychecks",
        create: "Create paycheck",
        created: "Successfully created paycheck",
        pay: "Confirm payment",
        paid: "Paycheck was marked as paid",
        amount: "Amount",
        isPaid: "Paid",
        intervalStart: "Interval start",
        intervalEnd: "Interval end",
        createdAt: "Created at"
    },
    paycheckBonus: {
        single: "Bonus paycheck",
        plural: "Bonus paychecks",
        create: "Create bonus",
        created: "Successfully created bonus paycheck",
    }
})
