using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Clone.Models
{
    public abstract class AuditableEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public uint xmin { get; set; } = 0;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        [MaxLength(128)]
        public string CreatedUser { get; set; } = "not logged";
        public DateTime? UpdatedOn { get; set; }
        [MaxLength(128)]
        public string? UpdatedUser { get; set; }

        public void CreatedAudit(ClaimsPrincipal? claims)
        {
            CreatedOn = DateTime.UtcNow;
            CreatedUser = claims?.Identity?.Name ?? "not logged";
        }

        public void UpdateAudit(ClaimsPrincipal? claims)
        {
            UpdatedOn = DateTime.UtcNow;
            UpdatedUser = claims?.Identity?.Name ?? "not logged";
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} Id: {Id} ";
        }
    }
}