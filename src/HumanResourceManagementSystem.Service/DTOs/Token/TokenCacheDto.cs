using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.Token
{
    public record TokenCacheDto(
    Guid UserId,
    string UserName,
    IEnumerable<Guid> RoleIds,
    string Token,
    DateTime CreatedAt,
    string DeviceInfo);
}
