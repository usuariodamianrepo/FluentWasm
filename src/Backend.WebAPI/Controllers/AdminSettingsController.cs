namespace Backend.WebAPI.Controllers
{
    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Settings;
    using Backend.Application.Services.Contracts.Admin;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/settings")]
    public class AdminSettingsController : ControllerBase
    {
        private readonly IAdminSettingsService _adminSettingsService;

        public AdminSettingsController(IAdminSettingsService adminSettingsService)
        {
            _adminSettingsService = adminSettingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _adminSettingsService.GetAsync());
        }

        [HttpPut("app")]
        public async Task<IActionResult> UpdateApp([FromBody] UpdateAppSettingsDto request)
        {
            var result = await _adminSettingsService.UpdateAppAsync(request);
            return result.Success ? Ok(result) : ToFailureResult(result);
        }


        private IActionResult ToFailureResult<TPayload>(ServiceResponse<TPayload> result)
        {
            return result.ResponseType switch
            {
                ServiceResponseType.NotFound => NotFound(result),
                ServiceResponseType.Conflict => Conflict(result),
                ServiceResponseType.ValidationError => BadRequest(result),
                _ => BadRequest(result),
            };
        }
    }
}
