using BuildingBlock.Core.Share.Helpers;
using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.DTOs.User;
using HumanResourceManagementSystem.Service.Exceptions.User;
using HumanResourceManagementSystem.Service.Interfaces;
using System.Security.Claims;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class UserService(IHumanResourceUnitOfWork db) : IUserService
    {
        private readonly IHumanResourceUnitOfWork _db = db;

        public async Task CreateUserAsync(CreateUserDto userDto)
        {
            byte[] salt = PasswordHelper.GenerateSalt();

            string hashPassword = PasswordHelper.HashPassword(userDto.Password, salt);

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = hashPassword,
                Salt = Convert.ToBase64String(salt),
                UserRoles = userDto.Roles.Select(r => new UserRole
                {
                    Role = new Role { Id = r.Id, Name = r.Name }
                }).ToList(),
                UserClaims = userDto.Claims.Select(c => new UserClaim
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value,
                }).ToList()
            };

            await _db.Users.AddAsync(user);
            await _db.CompleteAsync();
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto)
        {
            var user = await _db.Users.GetByIdAsync(userDto.Id) ?? throw new UserNotFoundException(userDto.Id);
            user.Email = userDto.Email;
            user.UserRoles = userDto.Roles.Select(r => new UserRole
            {
                Role = new Role { Id = r.Id, Name = r.Name }
            }).ToList();
            user.UserClaims = userDto.Claims.Select(c => new UserClaim
            {
                ClaimType = c.Type,
                ClaimValue = c.Value,
            }).ToList();

            await _db.Users.UpdateAsync(user);
            await _db.CompleteAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _db.Users.GetByIdAsync(userId) ?? throw new UserNotFoundException(userId);
            await _db.Users.RemoveAsync(user);
            await _db.CompleteAsync();
        }

        public async Task UpdatePasswordAsync(UpdateUserPasswordDto dto)
        {
            var user = await _db.Users.GetByIdAsync(dto.Id) ?? throw new UserNotFoundException(dto.Id);
            byte[] salt = PasswordHelper.GenerateSalt();
            string hashPassword = PasswordHelper.HashPassword(dto.NewPassword, salt);

            user.PasswordHash = hashPassword;
            user.Salt = Convert.ToBase64String(salt);

            await _db.Users.UpdateAsync(user);
            await _db.CompleteAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _db.Users.GetByIdAsync(userId);
            return user ?? throw new UserNotFoundException(userId);
        }
        
        public async Task<AuthenticationResultDto> AuthenticateAsync(LoginCredentialsDto dto)
        {
            var user = await _db.Users.GetUserByEmailAsync(dto.Email) 
                ?? throw new UserUnauthorizedException("Email",dto.Email);

            return new AuthenticationResultDto
            { 
                IsVerified = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash, Convert.FromBase64String(user.Salt)),
                UserId = user.Id,
                UserName = user.UserClaims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Name)?.ClaimValue ?? "",
                RoleIds = [.. user.UserRoles.Select(r => r.RoleId)]
            };
        }
    }
}
