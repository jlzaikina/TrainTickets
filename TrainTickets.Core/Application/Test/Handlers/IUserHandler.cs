using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface IUserHandler
{
    /// <summary>
    /// Получить все.
    /// </summary>
    /// <returns><see cref="UserDto"/></returns>
    Task<IEnumerable<UserDto>> GetAllUserAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task<bool> RegisterUserAsync(RegisterUserRequest request);
    Task<bool> AuthUserAsync(AuthUserRequest request);
}