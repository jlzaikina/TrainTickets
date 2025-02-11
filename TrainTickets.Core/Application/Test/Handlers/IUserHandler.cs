using TrainTickets.UI.Domain;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface IUserHandler
{
    /// <summary>
    /// Получить все.
    /// </summary>
    /// <returns><see cref="UserDto"/></returns>
    Task<IEnumerable<UserDto>> GetAllUserAsync();
    Task<UserDto> GetUserByIdAsync(int id);
}