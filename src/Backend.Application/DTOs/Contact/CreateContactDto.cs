namespace Backend.Application.DTOs.Contact
{
    public sealed class CreateContactDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Company { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
