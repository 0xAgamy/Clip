using Clips.Domain.Models;

namespace Clips.Application.Interfaces;

public interface IJwtService
{
    string CreateJwt(User user);
}
