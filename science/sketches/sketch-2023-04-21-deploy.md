Proposed file system structure:
```
- $HOME/fwsh.example.com/ # fwsh root directory

  - api/            # here are compiled artifacts of web api
  - api.conf        # configuration for api.fwsh.example.com vhost

  - cdn/            # here are uploaded content files (photos, docs...)
  - cdn.conf        # configuration for cdn.fwsh.example.com vhost

  - www/            # here are static files for customer's website
  - www.conf        # configuration for www.fwsh.example.com vhost

  - worker/         # here are static files for worker's panel
  - worker.conf     # configuration for worker.fwsh.example.com vhost

  - manager/        # here are static files for worker's panel
  - manager.conf    # configuration for manager.fwsh.example.com vhost

``` 

Prod build steps:
- configure .env for Fwsh.WebApi
- build Fwsh.WebApi in self-contained mode
- configure .env for Frontend/{Customer,Manager,Worker}
- build Frontend/{Customer,Manager,Worker}
- put everything into one dist directory

Prod deployment steps (user):
- put dist directory to server
- create root CA
- create certificates for vhosts

Prod deployment steps (root):
- add symlinks to conf files
- service apache2 restart
- create service for Fwsh.WebApi
- service Fwsh.WebApi start

Client configuration steps:
- copy root CA certificate to client's computer
- install root CA certificate locally
- add {www,manager,worker}.fwsh.example.com to /etc/hosts
- or configure dns server

Client usage:
- open {www,manager,worker}.fwsh.example.com in web browser
