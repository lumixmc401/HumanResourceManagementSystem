using HumanResourceManagementSystem.API.Jwt;
using HumanResourceManagementSystem.Service.Dtos.User;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IUserService service, JwtTokenGenerator jwt) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly JwtTokenGenerator _jwt = jwt;
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken(VerifyUserRequestDto dto)
        {
            var response = await _service.VerifyUser(dto);
            if (response.IsVerified)
            {
                return Ok(_jwt.GenerateJwtToken(response.UserId,response.UserName,response.RoleIds));
            }
            return Unauthorized();
        }
    }
}
