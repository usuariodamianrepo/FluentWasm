using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.DTOs.Contact
{
    public sealed class GetSearchContactDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Company { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
