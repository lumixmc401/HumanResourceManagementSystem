using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(UpdateUserDto user);
        Task UpdatePasswordAsync(UpdateUserPasswordDto user);
        Task<VerifyUserResponseDto> VerifyUserAsync(VerifyUserRequestDto dto);
        Task DeleteUserAsync(Guid userId);
    }
}
