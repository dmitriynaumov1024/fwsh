Some notes

i: Discount logic: Discount is determined per customer. Max. discount is to be about 30%. Simplest discount logic: after each successful order, customer's discount percent is increased by 2, until it reaches max discount percent.

i: In events, there is such thing as Balance. That's money balance. Positive values mean money inflow, in case when customers pay for the orders. Negative values mean money outflow, in case when workshop purchases materials, parts etc, or pay workers for their job. 

i: Daily production planning based on statistical data
Basically there can be 2 modes: 
- 'Freehand' production
- Planned production

Q: How many parts this system consists of?
A: This system consists of 4 parts:
- Server application (dotnet6)
- Customer's panel (vue3)
- Manager's panel (vue3)
- Worker's panel (vue3)

Q: How are parts accounted?
A: Storage is to be checked by workers, they can log amounts of parts/materials

Q: How to determine if resource is to be refilled?
A: Every resource has its mean refill period and normal stock amount - basically amount right after refill. If it's time to refill AND real amount drops below 60% OR any time amount drops below 30% of normal stock, then refill. 

Q: How should workers know what to do with design?
A: Old workers should know the design well, but for newcomers each design will have an instruction book. Also, for each design, there is a list of used parts and materials with all the quantities, so workers and managers know how much of anything is needed.

Q: How is order translated into work plan?
A: For each design, there is a list of task prototypes, and based on those prototypes, real tasks are created, and when task is created, a worker is assigned to it.

Q: How are workers assigned to production tasks?
A: Based on their roles and already existing load.

Q: How is price of design determined?
A: Design price is Resouce price + Worker payment. Resource price is just a sum of resource prices of each production task. Recalculation is to be triggered once every day. Worker payment is manually set up depending on worker's opinions. Once ordered, design price for customer is fixed and shall not be changed unless a force majeure event occurs.

Q: How is repair order payment determined?
A: It is determined individually per order. There should be a prepayment to guarantee reliability of customer. Prepayment is to be 25% of estimated cost. For efficient cost estimation, customer should provide description and photos of furniture item and its problems.  

Q: How resource usage works?
A: While task status is "working" it is locked on that worker. When worker finishes, they can set resource usage (mandatory) in case of repair task, or overwrite default values in case of production task. And then task status should be manually set to "finished". Task can be reopened by worker in 10 minutes after finishing, or by manager any time.

Q: How can worker reject a task?
A: Task can be rejected by setting its status to "rejected". Then system should assign the task to another worker.

