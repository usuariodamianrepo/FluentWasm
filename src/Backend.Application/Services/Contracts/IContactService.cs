using Backend.Application.DTOs;
using Backend.Application.DTOs.Contact;
using Backend.Domain.Contracts;

namespace Backend.Application.Services.Contracts
{
    public interface IContactService
    {
        Task<IEnumerable<GetContactDto>> GetAllAsync();
        Task<PagedResult<GetSearchContactDto>> GetSearchPageAsync(string name);
        Task<GetContactDto?> GetByIdAsync(Guid id);
        Task<ServiceResponse> AddAsync(CreateContactDto contact);
        Task<ServiceResponse> UpdateAsync(UpdateContactDto contact);
        Task<ServiceResponse> DeleteAsync(Guid id);
    }
}
