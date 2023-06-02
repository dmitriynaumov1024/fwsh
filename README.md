# fwsh

Production management and order processing system for a furniture workshop

Copyright 2022 - 2023 Dmitriy Naumov naumov1024@gmail.com

## Technology stack

- Database: [PostgreSQL](https://www.postgresql.org/) 
  or [in memory](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/)
- Backend: WebAPI made with [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- Frontend: SPA made with [Vue.js](https://vuejs.org/) 

## Local deployment

Prerequisites:
- postgresql 14
- dotnet 6
- node 18
- npm 8

Deployment steps
1. Create database
2. Configure .env in Fwsh.MockData using .env.example
3. Seed database 
```
cd Fwsh.MockData
dotnet run -- seed log-credentials > credentials.log
```
4. Configure .env in Fwsh.WebApi using .env.example
5. Build and launch Backend
```
cd Fwsh.WebApi
dotnet run
```
6. Launch Frontend SPA for &lt;user&gt; = ( Customer | Manager | Worker )
```
cd Frontend
npm i -w <user>
npm run dev -w <user>
```
7. Open [`http://localhost:5000/`](http://localhost:5000/) in your browser.

## Production deployment

Server prerequisites: 
- apache2, apache2-ssl, apache2-proxy
- postgresql
- rsync

Dev prerequisites:
- dotnet 6
- node 18
- npm 8
- rsync

How to build and deploy:
1. Go to repository root directory.
2. Exec `./deploy/interactive.sh` . Initially you will be prompted for 
   server IP, server user name, etc. Then you will be prompted to configure
   .env files for Fwsh.WebApi and Frontend. Also you will be prompted for
   server user password, server root password and (?) local root password.
3. Install rootca.crt certificate in your browser.
4. Open [`https://www.fwsh.example.com`](https://www.fwsh.example.com) in your browser. 
