ResourcePurchaseEvent - to track resource purchases and money spent on those.

ResourceUsageEvent - to track resource usage, even though most resources are accounted in TaskPart / TaskMaterial / TaskFabric, there can be some special cases. Also, in case of Repair, resource usage can only be accounted manually.

Signup event - to account new customers and have some statistical infographics.

ProductionOrderEvent - to account design/fabric popularity in given time interval.

RepairOrderEvent - just to count repair orders.

{Production,Repair}PaymentEvent - to track order payments and money inflow.

PaycheckReceiveEvent - to track paychecks and money outflow.

ProductionEvent - to track status changes of production orders.

RepairEvent - to track status changes of repair orders.

ProductionTaskCompletionEvent - useless?
