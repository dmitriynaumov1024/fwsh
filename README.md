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
7. Open [`http://localhost:5000/`](http://localhost:5000/) in web browser
