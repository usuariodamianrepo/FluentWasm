namespace Frontend.Shared.Models
{
    public record LoginResponse(bool Success = false, string Message = null!, string Token = null!);
}
