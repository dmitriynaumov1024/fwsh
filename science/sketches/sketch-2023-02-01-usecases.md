Customer can:

- profile
  - sign up
  - log in
  - edit
  - view

- catalog
  - view design list
    - by type
  - view one design
  - view decor material list
    - by color
  - view fabric list
    - by color
    - by type

- order
  - create production order (design id, fabric id, decor id, quantity)
  - create repair order (description, photos)
  - view production orders
    - only active
    - only completed
  - view repair orders
    - only active
    - only completed
  - view one production order
  - view one repair order


Worker can:

- profile
  - sign up
  - log in
  - view

- production task
  - view tasks assigned to themselves
    - active
    - archive
  - view one task
  - set task status
  - send a notification
  - set resource usage to overwrite default values

- repair task
  - view tasks assigned to themselves
    - active
    - archive
  - view one task
  - set task status
  - send a notification
  - set resource usage

- part
  - update stock quantity

- material
  - update stock amount

- fabric 
  - update stock amount


Manager can:

- design
  - create
  - view all
  - view one
  - edit design
  - associated task prototypes
    - view all
    - create
    - edit 
    - delete
    - associated resource quantities
      - view all
      - create
      - edit
      - delete 
  - associated photos
    - create
    - delete

- color
  - view all
  - find by name
  - create
  - edit
  - delete

- fabric type
  - view all
  - find by name
  - create
  - edit
  - delete

- fabric
  - view all
  - find by name
  - create
  - edit
  - delete

- material
  - view all
  - find by name
  - create
  - edit
  - delete

- part
  - view all
  - find by name
  - create
  - edit
  - delete

- supply order
  - view all (list|archive)
  - view one
  - create
  - edit
  - confirm submit
  - set status (received|impossible)

- production task
  - view one
  - view all (design id)
  - create
  - edit
  - delete

- repair task
  - view all
  - view by worker
  - view one
  - create
  - edit
  - delete? 

- production order
  - view all
  - view only active
  - find by customer
  - find by same design
  - view one
  - set status
  - notify
  - create prod.tasks

- repair order
  - view all
  - view only active
  - find by customer
  - set status
  - notify
  - create repair tasks

