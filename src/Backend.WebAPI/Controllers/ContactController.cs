using Backend.Application.DTOs.Contact;
using Backend.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetContactDto>>> GetAll()
        {
            var contacts = await _contactService.GetAllAsync();
            return this.Ok(contacts);
        }

        [HttpGet("single/{id}")]
        public async Task<ActionResult<GetContactDto?>> GetById(Guid id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            return contact != null ? this.Ok(contact) : this.NotFound();
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(CreateContactDto contact)
        {
            var result = await _contactService.AddAsync(contact);
            return result.Success ? this.Ok(result) : this.BadRequest(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateContactDto contact)
        {
            var result = await _contactService.UpdateAsync(contact);
            return result.Success ? this.Ok(result) : this.BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contactService.DeleteAsync(id);
            return result.Success ? this.Ok(result) : this.BadRequest(result.Message);
        }
    }
}