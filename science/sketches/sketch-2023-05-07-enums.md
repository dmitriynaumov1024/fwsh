Various enums used in Furniture workshop to describe status or type

FurnitureType = 
    | 'unknown'
    | 'sofa'
    | 'minisofa'
    | 'ottoman'
    | 'corner'
    | 'armchair'
    | 'pouffe'

ResourceType = 
    | 'resource'
    | 'part'
    | 'material'
    | 'fabric'

MeasureUnit =
    | null
    | 'mm'
    | 'cm'
    | 'm'
    | 'm2'
    | 'm3'
    | 'L'
    | 'Kg'
    | 'g'

OrderStatus = 
    | 'unknown' => 'submitted'
    | 'submitted' => 'delayed', 'impossible'
    | 'delayed' => 'working', 'impossible'
    | 'working' => 'finished', 'impossible'
    | 'finished' => 'received', 'rejected'
    | 'received'
    | 'rejected'
    | 'impossible'

TaskStatus =
    | 'unknown' => 'assigned'
    | 'assigned' => 'working', 'rejected', 'impossible'
    | 'working' => 'finished', 'impossible'
    | 'rejected' => 'assigned'
    | 'finished'
    | 'impossible'

WorkerRole = 
    | 'unknown'
    | 'management'
    | 'sewing'
    | 'carpentry'
    | 'assembly'
    | 'upholstery'

SlotName = 
    | null
    | 'fabric'
    | 'decor'
