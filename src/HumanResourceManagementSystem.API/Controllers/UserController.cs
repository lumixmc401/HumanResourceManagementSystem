using HumanResourceManagementSystem.Service.Dtos.User;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            await _userService.CreateUserAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            await _userService.UpdateUserAsync(dto);
            return Ok();
        }

        [HttpPut("Password")]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordDto dto)
        {
            await _userService.UpdatePasswordAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
