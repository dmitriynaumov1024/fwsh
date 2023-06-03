1) Conduct interview
Results of interview: we need a production management and order processing system for a furniture workshop.

2) Use case model

Actors: 

- Customer
- Worker
- Manager

Use cases

- Customer
    - Profile
        - Sign up
        - Edit profile
        - Log in
        - Log out
    - Production Orders
        - Create order
        - Browse orders
        - Reject order
        - Delete order (when unknown or submitted)
    - Repair orders
        - Create order
        - Browse orders
        - Delete order (when unknown or submitted)
    - Designs
        - Browse designs
        - View one design
    - Colors
        - Browse colors
    - Fabric types
        - Browse fabric types
    - Fabrics
        - Browse fabrics
    - Materials
        - Browse decor materials

- Worker
    - Profile
        - Sign up
        - Edit profile
        - Log in
        - Log out
    - Production Tasks
        - Browse tasks
        - Set task status (working, finished, rejected, impossible)
        - Set resource usage
    - Repair Tasks
        - Browse tasks
        - Set task status (working, finished, rejected, impossible)
        - Set resource usage
    - Stored parts
        - Browse
        - Set quantity
    - Stored materials
        - Browse 
        - Set quantity
    - Stored fabrics
        - Browse
        - Set quantity
    - Paychecks
        - Browse
        - Generate paycheck

- Manager
    - Profile
        - Sign up
        - Edit profile
        - Log in
        - Log out
    - Production Tasks
        - Browse tasks
        - Set task status (working, finished, rejected, impossible)
    - Repair Tasks
        - Browse tasks
        - Set task status (working, finished, rejected, impossible)
    - Production orders
        - Browse orders (active, archive)
        - Set order status (delayed, working, finished, impossible)
    - Repair orders
        - Browse orders (active, archive)
        - Set order status (delayed, working, finished, impossible)
    - Designs
        - Browse designs
        - View one design
        - Create/edit design
    - Colors
        - Browse colors
        - Create/edit color
    - Fabric types
        - Browse fabric types
        - Create/edit fabric type
    - Fabrics
        - Browse fabrics
        - Create/edit fabric
    - Materials
        - Browse materials
        - Create/edit material
    - Parts
        - Browse parts
        - Create/edit part
    - Supply orders
        - List
        - View one
        - Create/edit
        - Submit/Receive
    - Customers
        - List
    - Suppliers
        - List
        - View one
        - Create/edit
    - Workers
        - List
        - View one
    - Paychecks
        - List
        - Archive
        - Create
        - Create bonus
        - Confirm payment

