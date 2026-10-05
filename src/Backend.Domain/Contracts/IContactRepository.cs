using Backend.Domain.Entities;

namespace Backend.Domain.Contracts
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetSearchContactsAsync(string name);
    }
}
