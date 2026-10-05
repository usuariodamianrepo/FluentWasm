### Changelog Backend Proyect
## Cronograma
10:36 24/9/2026
- CORS allowed origins: https://localhost:7258
- Now listening on: http://localhost:5282
- Request starting HTTP/1.1 GET http://localhost:5282/api/Contact/all - null null
Validando la conexión
"https://localhost:7094",
"http://localhost:5282",
	[✅] http://localhost:5282/scalar/#tag/authentication
	[✅] http://localhost:5282/scalar/scalar.js 
	[✅] http://localhost:5282/openapi/v1.json
aca es donde quiere conectar wasm
	[❌] https://localhost:7192/api/openapi/v1.json 
	
	
---
donde publica API https://localhost:7094/swagger/v1/swagger.json
donde busca WASP https://localhost:7258/api/

19:01 19/8/2026
- Domain: agregue AppUser => dependencia
	dotnet add Backend.Domain/Backend.Domain.csproj package Microsoft.Extensions.Identity.Stores
- Domain: agregue Authentication y los Result
- Infra: agregue AppDbContext => dependencias
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Microsoft.EntityFrameworkCore
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Microsoft.AspNetCore.Identity.UI (lo tube que agregar me tiraba error en la inyeccion de dependecias)
- Infra: agregamos Repositories => dependencias
	dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
	con esto se corrigió lo de los tokes 
- App: agrego Validation => dependencias
	[✅] dotnet add Backend.Application/Backend.Application.csproj package FluentValidation
	[✅] dotnet add Backend.Application/Backend.Application.csproj package FluentValidation.DependencyInjectionExtensions
15:31 20/8/2026
- App: agregue AuthenticationService TODO...
- App: agregar Mapster
	[✅] dotnet add Backend.Application/Backend.Application.csproj package Mapster
	[✅] dotnet add Backend.Application/Backend.Application.csproj package Mapster.DependencyInjection
- App: modificar Program.csproj
	agregué todos estos paquetes, se usan en WebAPI => TODO: revisar este tema
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Serilog
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Serilog.AspNetCore
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Serilog.Sinks.Console
	[✅] dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package Serilog.Sinks.File
	
	 <PackageReference Include="MailKit" Version="4.16.0" />
	 <PackageReference Include="MimeKit" Version="4.16.0" />
	 dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package MailKit
	 dotnet add Backend.Infrastructure/Backend.Infrastructure.csproj package MimeKit
10:16 21/8/2026
- WebAPI: agrege paquete para ejecutar las APIs Scalar.AspNetCore mas moderno y mejor que Swashbuckle.AspNetCore
	- [✅] dotnet add Backend.WebAPI/Backend.WebAPI.csproj package Scalar.AspNetCore	
18:55 24/8/2026
- WebAPI: agregue referancia para poder generar las tablas
	- [✅] dotnet add Backend.WebAPI/Backend.WebAPI.csproj package Microsoft.EntityFrameworkCore.Design
09:07 27/8/2026
- WebAssembly: agregue Fluent proyecto
	- [✅] dotnet add Frontend.WebAssembly/Frontend.WebAssembly.csproj package Microsoft.AspNetCore.Components.Authorization
	- [✅] dotnet add Frontend.WebAssembly/Frontend.WebAssembly.csproj package System.IdentityModel.Tokens.Jwt
	- [✅] dotnet add Frontend.WebAssembly/Frontend.WebAssembly.csproj package 	
- WebAssembly.Shared: agregue Library proyecto
	- [✅] dotnet add Frontend.WebAssembly.Shared/Frontend.WebAssembly.Shared.csproj package Microsoft.AspNetCore.Components.Web
	- [✅] dotnet add Frontend.WebAssembly.Shared/Frontend.WebAssembly.Shared.csproj package Microsoft.Extensions.Http	
	- [✅] dotnet add Frontend.WebAssembly.Shared/Frontend.WebAssembly.Shared.csproj package Microsoft.JSInterop
08:57 2/9/2026
- agregue a la entidad Contact y AdminAuditLog
09:24 3/9/2026
- /init --create AGENTS.md
	[Agent][interactive][GPT-5.6 Luna]
- i need a plan with the changes necessaries to improvement the security in `src/Backend.WebAPI`
	[Plan][Interactive][MAI-Code-1.1-Flash]
16:13 4/9/2026
- agregue xUnit test
	- [✅] dotnet add Frontend.WebAssembly/Frontend.WebAssembly.csproj package Microsoft.AspNetCore.Components.Authorization
    <PackageReference Include="Aspire.Hosting.Testing" Version="13.2.2" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.6" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.0.5" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.0.1" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.5">
	
	  <ItemGroup>
    <ProjectReference Include="..\BlazorShop.AppHost\BlazorShop.AppHost.csproj" Aliases="AppHost" />
    <ProjectReference Include="..\BlazorShop.Application\BlazorShop.Application.csproj" />
    <ProjectReference Include="..\BlazorShop.Infrastructure\BlazorShop.Infrastructure.csproj" />
    <ProjectReference Include="..\BlazorShop.Presentation\BlazorShop.API\BlazorShop.API.csproj" />
    <ProjectReference Include="..\BlazorShop.Presentation\BlazorShop.Web\BlazorShop.Web.csproj" />
    <ProjectReference Include="..\BlazorShop.Presentation\BlazorShop.Web.Shared\BlazorShop.Web.Shared.csproj" />
    <ProjectReference Include="..\BlazorShop.Presentation\BlazorShop.Storefront\BlazorShop.Storefront.csproj" />
  </ItemGroup>
  
## Pasos para agregar una Entidad
1. Domain
	- D:\gitdev\BoilerplateAPI\src\Backend.Domain\Entities\Contact.cs
	- D:\gitdev\BoilerplateAPI\src\Backend.Domain\Contracts\IContactRepository.cs
2. Infrastructure
	- D:\gitdev\BoilerplateAPI\src\Backend.Infrastructure\Data\AppDbContext.cs
	- D:\gitdev\BoilerplateAPI\src\Backend.Infrastructure\Repositories\ContactRepository.cs
3. Application
	- D:\gitdev\BoilerplateAPI\src\Backend.Application\Services\ContactService.cs
	- D:\gitdev\BoilerplateAPI\src\Backend.Application\Services\Contracts\IContactService.cs

	[GPT-5 mini]
	
i need a plan with the changes necessaries to improvement the security in `src/Backend.WebAPI`

add-feature
## Step 1 — Add the Entity in AppDbContext

## Step 2 — Add the repository and its interface

## Step 3 - Add the Service

add-crud