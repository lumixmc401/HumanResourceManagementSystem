using HumanResourceManagementSystem.Api.Jwt;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IUserService service, JwtTokenGenerator jwt) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly JwtTokenGenerator _jwt = jwt;
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken(VerifyUserDto dto)
        {
            var response = await _service.VerifyUserAsync(dto);
            if (response.IsVerified)
            {
                return Ok(_jwt.GenerateJwtToken(response.UserId,response.UserName,response.RoleIds));
            }
            return Unauthorized();
        }
    }
}
