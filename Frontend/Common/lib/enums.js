const OrderStatus = {
    unknown: "unknown",
    submitted: "submitted",
    delayed: "delayed",
    working: "working",
    finished: "finished",
    received: "received",
    rejected: "rejected",
    impossible: "impossible"
}

const TaskStatus = {
    unknown: "unknown",
    assigned: "assigned",
    working: "working",
    finished: "finished",
    rejected: "rejected",
    impossible: "impossible"
}

const FurnitureTypes = {
    notSelected: null,
    unknown: "unknown",
    sofa: "sofa",
    minisofa: "minisofa",
    ottoman: "ottoman",
    corner: "corner",
    armchair: "armchair",
    pouffe: "pouffe"
}

const MeasureUnits = {
    unknown: null,
    millimeters: "mm",
    centimeters: "cm",
    meters: "m",
    squareMeters: "m2",
    cubicMeters: "m3",
    liters: "L",
    kilograms: "Kg",
    grams: "g"
}

const ResourceTypes = {
    resource: "resource",
    part: "part",
    material: "material",
    fabric: "fabric"
}

const SlotNames = {
    unknown: null,
    fabric: "fabric",
    decor: "decor"
}

const WorkerRoles = {
    unknown: "unknown",
    management: "management",
    sewing: "sewing",
    carpentry: "carpentry",
    assembly: "assembly",
    upholstery: "upholstery"
} 

export {
    OrderStatus,
    TaskStatus,
    FurnitureTypes,
    MeasureUnits,
    ResourceTypes,
    SlotNames,
    WorkerRoles,
}
