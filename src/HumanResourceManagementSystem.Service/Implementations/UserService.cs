using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks;
using HumanResourceManagementSystem.Service.Dtos.User;
using HumanResourceManagementSystem.Service.Exceptions.User;
using HumanResourceManagementSystem.Service.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHumanResourceUnitOfWork _db;

        public UserService(IHumanResourceUnitOfWork db)
        {
            _db = db;
        }

        public async Task CreateUserAsync(CreateUserDto userDto)
        {
            byte[] salt = GenerateSalt();

            string hashPassword = HashPassword(userDto.Password, salt);

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
                    Claim = new Claim { Id = c.Id, Type = c.Type, Value = c.Value }
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
                Claim = new Claim { Id = c.Id, Type = c.Type, Value = c.Value }
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

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _db.Users.GetByIdAsync(userId) ?? throw new UserNotFoundException(userId);
            byte[] salt = GenerateSalt();
            string hashPassword = HashPassword(newPassword, salt);

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

        private string HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // 4 cores
                MemorySize = 1024 * 1024, // 1 GB
                Iterations = 4
            };

            return Convert.ToBase64String(argon2.GetBytes(16));
        }

        private byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
