using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure.Repositories
{

    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Contact>> GetSearchContactsAsync(string name)
        {
            return await _context.Contacts.AsNoTracking()
                .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
                .ToListAsync(); 
        }
    }
}
