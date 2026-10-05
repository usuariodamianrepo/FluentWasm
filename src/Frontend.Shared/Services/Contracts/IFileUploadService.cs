namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;

    using Microsoft.AspNetCore.Components.Forms;

    public interface IFileUploadService
    {
        Task<FileUploadResponse> UploadFileAsync(IBrowserFile file);
    }
}
