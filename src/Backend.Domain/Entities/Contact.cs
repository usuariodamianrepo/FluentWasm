using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
    public partial class Contact
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Email { get; set; } = null!;
        [MaxLength(60)]
        public string? Company { get; set; }
        [Required]
        [MaxLength(60)]
        public string LastName { get; set; } = null!;
        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; } = null!;
        [MaxLength(60)]
        public string? Phone { get; set; }

        public string Name => $"{FirstName} {LastName}".Trim();

    }
}