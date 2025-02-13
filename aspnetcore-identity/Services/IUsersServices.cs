using aspnetcore_identity.Models.Dtos;

namespace aspnetcore_identity.Services;

public interface IUsersServices
{
    Task<IResult> CreateUserAsync(UserRegisterDto request, CancellationToken cancellationToken);
}