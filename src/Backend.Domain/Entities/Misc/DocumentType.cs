
using Clone.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities.Misc
{
    public sealed class DocumentType : AuditableEntity
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = default!;
        [Required]
        [MaxLength(120)]
        public string Description { get; set; } = default!;
    }
}