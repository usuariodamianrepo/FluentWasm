# Agent Guide

## Solution

- `BoilerplateAPI.slnx` contains a .NET 10 API split into four projects.
- `src/Backend.WebAPI` is the ASP.NET Core entry point. Controllers, middleware registration, health checks, options, and `Program.cs` belong here.
- `src/Backend.Application` contains DTOs, application services/contracts, validation, mapping, and application exceptions.
- `src/Backend.Domain` contains entities and domain contracts. Keep it independent of infrastructure concerns.
- `src/Backend.Infrastructure` contains EF Core/PostgreSQL persistence, Identity/JWT, repositories, email, logging, middleware, and migrations.

## Dependency direction

`Backend.WebAPI` references `Backend.Infrastructure`; `Backend.Infrastructure` references `Backend.Application`; `Backend.Application` references `Backend.Domain`. Preserve this direction and use interfaces from Application or Domain when adding implementations.

## Working conventions

- Target `net10.0`; nullable reference types and implicit usings are enabled.
- Follow the existing controller/service/repository patterns and use the existing `ServiceResponse` types, FluentValidation, Mapster, Serilog, and EF Core setup.
- Update EF Core migrations when changing the persisted model. Do not edit generated migration or snapshot files manually unless required by the migration workflow.
- Keep secrets and environment-specific values out of source control; use user secrets or environment configuration.

## Validation

- Build with `dotnet build BoilerplateAPI.slnx`.
- Run the API with `dotnet run --project src/Backend.WebAPI/Backend.WebAPI.csproj`.
- Use `src/Backend.WebAPI/Backend.WebAPI.http` for basic endpoint checks. No test project is currently included in the solution.
