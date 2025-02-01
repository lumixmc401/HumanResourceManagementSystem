using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Service.Dtos.User;
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
        Task<bool> VerifyUser(VerifyUserDto dto);
        Task DeleteUserAsync(Guid userId);
    }
}
