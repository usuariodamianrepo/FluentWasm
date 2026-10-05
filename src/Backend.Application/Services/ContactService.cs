using Backend.Application.DTOs;
using Backend.Application.DTOs.Admin.Audit;
using Backend.Application.DTOs.Contact;
using Backend.Application.Services.Contracts;
using Backend.Application.Services.Contracts.Admin;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using MapsterMapper;
using System.Text.Json;

namespace Backend.Application.Services
{
    public class ContactService: IContactService
    {
        private readonly IAdminAuditService? _auditService;
        private readonly IContactRepository _contactRepository;
        private readonly IGenericRepository<Contact> _genericRepository;
        private readonly IMapper _mapper;
        public ContactService(IGenericRepository<Contact> genericRepository, IMapper mapper, IContactRepository contactRepository, IAdminAuditService? auditService = null)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _contactRepository = contactRepository;
            _auditService = auditService;
        }

        public async Task<IEnumerable<GetContactDto>> GetAllAsync()
        {
            var result = await _genericRepository.GetAllAsync();
            return result.Any() ? _mapper.Map<IEnumerable<GetContactDto>>(result) : new List<GetContactDto>();
        }

        public async Task<PagedResult<GetSearchContactDto>> GetSearchPageAsync(string name)
        {
            var result = await _contactRepository.GetSearchContactsAsync(name);
            var mappedItems = _mapper.Map<IReadOnlyList<GetSearchContactDto>>(result);

            return new PagedResult<GetSearchContactDto>
            {
                Items = mappedItems,
                PageNumber = 1,
                PageSize = mappedItems.Count(),
                TotalCount = mappedItems.Count(),
            };
        }

        public async Task<GetContactDto?> GetByIdAsync(Guid id)
        {
            var result = await _genericRepository.GetByIdAsync(id);
            return result != null ? _mapper.Map<GetContactDto>(result) : null;
        }

        public async Task<ServiceResponse> AddAsync(CreateContactDto contact)
        {
            var mappedData = _mapper.Map<Contact>(contact);
            int result = await _genericRepository.AddAsync(mappedData);

            if (result <= 0)
            {
                return new ServiceResponse(false, "Contact not added");
            }

            await LogAsync("Contact.Created", mappedData.Id, $"Contact {mappedData.FirstName} {mappedData.LastName} created.", new { mappedData.FirstName, mappedData.LastName, mappedData.Email });
            return new ServiceResponse(true, "Contact added successfully", mappedData.Id);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateContactDto contact)
        {
            var existingContact = await _genericRepository.GetByIdAsync(contact.Id);

            if (existingContact is null)
            {
                return new ServiceResponse(false, "Contact not found");
            }

            _mapper.Map(contact, existingContact);
            int result = await _genericRepository.UpdateAsync(existingContact);

            if (result <= 0)
            {
                return new ServiceResponse(false, "Contact not found");
            }

            await LogAsync("Contact.Updated", existingContact.Id, $"Contact {existingContact.Name} updated.", new { existingContact.Name });
            return new ServiceResponse(true, "Contact updated successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var result = await _genericRepository.DeleteAsync(id);

            if (result == 0 )
            {
                return new ServiceResponse(false, "Contact not found");
            }

            await LogAsync("Contact.Deleted", id, $"Contact {id} deleted.", new { Name = nameof(Contact) });
            return new ServiceResponse(true, "Contact deleted successfully");
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
                EntityType = nameof(Contact),
                EntityId = entityId.ToString(),
                Summary = summary,
                MetadataJson = JsonSerializer.Serialize(metadata),
            });
        }
    }
}
