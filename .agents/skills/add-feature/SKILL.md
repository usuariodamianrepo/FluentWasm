---
name: add-feature
description: Add all code necessary feature (repository + DTO + validator + endpoint) from an Entity. Use when adding an API endpoint or business operation to a module that already exists.
argument-hint: "[Entity]"
---

# Add Feature

A feature is all code necessary to create a CRUD for the Backend. The argument is an Entity to define the different variables definition into the code generated. It use the entity name in plural and singular name.

## Layout (real)

```
src/Modules/{X}/Modules.{X}.Contracts/v1/{Area}/{Feature}Command.cs   # ICommand<T>/IQuery<T>
src/Modules/{X}/Modules.{X}.Contracts/Dtos/{Entity}Dto.cs             # response DTOs (if any)
src/Modules/{X}/Modules.{X}/Features/v1/{Area}/{Feature}/
├── {Feature}CommandHandler.cs    # public sealed, injects the DbContext directly
├── {Feature}CommandValidator.cs  # required for commands + paginated queries
└── {Feature}Endpoint.cs          # internal static extension
```

## Step 1 — Register the Entity in the module DbContext
Add a `DbSet` in the DbContext that is in `Backend.Infrastructure.Data`

```csharp
public DbSet<{EntitySingularName}> {EntityPluralName} { get; set; }
```

## Step 2 - Add the Repository files
Create repository file and its interface file.

1. create repository interface file in the `src/Backend.Domain/Contracts/`, use the follow template

```csharp
namespace Backend.Domain.Contracts
{
    public interface I{EntitySingularName}Repository
    {
    }
}
```

2. create repository file in the `src/Backend.Infrastructure/Repositories/`, use the follow template

```csharp
using Backend.Domain.Contracts;
using Backend.Infrastructure.Data;

namespace Backend.Infrastructure.Repositories
{
    public class {EntitySingularName}Repository : I{EntitySingularName}Repository
    {
        private readonly AppDbContext _context;

        public {EntitySingularName}Repository(AppDbContext dbContext)
        {
            _context = dbContext;
        }
    }
}
```

## Step 3 - Create the DTO files
Create DTO foler and its files.

1. create the {EntitySingularName} folder in the `src/Backend.Application/DTOs/` folder.

2. create all DTOs used in the CRUD operations in the `src/Backend.Application/DTOs/{EntitySingularName}/` folder, create the next files with the same properties that the Entity
	- Get{EntitySingularName}Dto.cs
	- GetSearch{EntitySingularName}Dto.cs
	- Create{EntitySingularName}Dto.cs
	- Update{EntitySingularName}Dto.cs

## Step 4 - Add the Service files
Create service file and its interface file.

1. create service interface file in the `src/Backend.Application/Services/Contracts/`, use the follow template
```csharp
using Backend.Domain.Entities;

namespace Backend.Domain.Contracts
{
    public interface I{EntitySingularName}Repository
    {

    }
}
```

2. create repository file in the `src/Backend.Application/Services/`, use the follow template

```csharp
using Backend.Application.DTOs;
using Backend.Application.DTOs.Admin.Audit;
using Backend.Application.DTOs.{EntitySingularName};
using Backend.Application.Services.Contracts;
using Backend.Application.Services.Contracts.Admin;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using MapsterMapper;
using System.Text.Json;

namespace Backend.Application.Services
{
    public class {EntitySingularName}Service: I{EntitySingularName}Service
    {
        private readonly IAdminAuditService? _auditService;
        private readonly IGenericRepository<{EntitySingularName}> _genericRepository;
        private readonly IMapper _mapper;
        public ContactService(IGenericRepository<{EntitySingularName}> genericRepository, IMapper mapper, IAdminAuditService? auditService = null)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task<IEnumerable<Get{EntitySingularName}Dto>> GetAllAsync()
        {
            var result = await _genericRepository.GetAllAsync();
            return result.Any() ? _mapper.Map<IEnumerable<Get{EntitySingularName}Dto>>(result) : new List<Get{EntitySingularName}Dto>();
        }

        public async Task<GetContactDto?> GetByIdAsync(Guid id)
        {
            var result = await _genericRepository.GetByIdAsync(id);
            return result != null ? _mapper.Map<Get{EntitySingularName}Dto>(result) : null;
        }

        public async Task<ServiceResponse> AddAsync(Create{EntitySingularName}Dto {EntitySingularNameCamelCase})
        {
            var mappedData = _mapper.Map<{EntitySingularName}>({EntitySingularNameCamelCase});
            int result = await _genericRepository.AddAsync(mappedData);

            if (result <= 0)
            {
                return new ServiceResponse(false, "{EntitySingularName} not added");
            }

            await LogAsync("{EntitySingularName}.Created", mappedData.Id, $"{EntitySingularName} {mappedData.Name} created.", new { mappedData.Name });
            return new ServiceResponse(true, "{EntitySingularName} added successfully", mappedData.Id);
        }

        public async Task<ServiceResponse> UpdateAsync(Update{EntitySingularName}Dto {EntitySingularNameCamelCase})
        {
            var existing{EntitySingularName} = await _genericRepository.GetByIdAsync({EntitySingularNameCamelCase}.Id);

            if (existing{EntitySingularName} is null)
            {
                return new ServiceResponse(false, "{EntitySingularName} not found");
            }

            _mapper.Map({EntitySingularNameCamelCase}, existing{EntitySingularName});
            int result = await _genericRepository.UpdateAsync(existing{EntitySingularName});

            if (result <= 0)
            {
                return new ServiceResponse(false, "{EntitySingularName} not found");
            }

            await LogAsync("{EntitySingularName}.Updated", existing{EntitySingularName}.Id, $"{EntitySingularName} {existing{EntitySingularName}.Name} updated.", new { existing{EntitySingularName}.Name });
            return new ServiceResponse(true, "{EntitySingularName} updated successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await _genericRepository.DeleteAsync(id);

            if (result == 0 )
            {
                return new ServiceResponse(false, "{EntitySingularName} not found");
            }

            await LogAsync("{EntitySingularName}.Deleted", id, $"{EntitySingularName} {id} deleted.", new { Name = nameof({EntitySingularName}) });
            return new ServiceResponse(true, "{EntitySingularName} deleted successfully");
        }

        private async Task LogAsync(string action, Guid entityId, string summary, object metadata)
        {
            if (_auditService is null)
            {
                return;
            }

            await _auditService.LogAsync(new CreateAdminAuditLogDto
            {
                Action = action,
                EntityType = nameof({EntitySingularName}),
                EntityId = entityId.ToString(),
                Summary = summary,
                MetadataJson = JsonSerializer.Serialize(metadata),
            });
        }
    }
}
```

## Compile proyect

Run the following command to compile the project. Only use this command when finished with all the changes, otherwise you will get errors.

```bash
dotnet build
```