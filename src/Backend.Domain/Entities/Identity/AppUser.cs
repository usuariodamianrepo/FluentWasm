namespace Backend.Domain.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class AppUser : IdentityUser
    {
        [MaxLength(60)]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool RequirePasswordChange { get; set; }
    }
}
