using Clone.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities.Misc
{
    public sealed class Document : AuditableEntity
    {
        [Required]
        [MaxLength(60)]
        public string Title { get; set; } = default!;
        [Required]
        [MaxLength(240)]
        public string Description { get; set; } = default!;
        public bool IsPublic { get; set; } = false;
        [Required]
        [MaxLength(360)]
        public string URL { get; set; } = default!;
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; } = default!;
    }
}