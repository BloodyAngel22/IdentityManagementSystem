# IdentityManagementSystem

## Description

> IdentityManagementSystem is a simple ASP.NET Core Web API that can be used to manage users and roles.

## Dependencies

- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore/)
- [Microsoft.AspNetCore.Identity.UI](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.UI/)
- [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/)

## Usage

- PostgreSQL

## How to run

- First you need to create a postgres database:

```sh
sudo -u postgres psql
```

```psql
CREATE DATABASE identitymanagementsystem;
```

- Then you need update migrations:

```sh
dotnet ef database update 
```

- Then you can run the application:

```bash
dotnet build
dotnet run
```
