namespace Backend.Application.DTOs.Admin.Users
{
    public class UpdateUserRolesDto
    {
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
    }
}
