using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <summary>
/// Работа с пользователем
/// </summary>
public interface IUserHandler
{
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns><see cref="UserDto"/></returns>
    Task<IEnumerable<UserDto>> GetAllUserAsync();

    /// <summary>
    /// Получить пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns><see cref="UserDto"/></returns>
    Task<UserDto> GetUserByIdAsync(int id);

    /// <summary>
    /// Регистрация
    /// </summary>
    /// <param name="request">Запрос на регистрацию</param>
    /// <returns>Истинность регистрации</returns>
    Task<bool> RegisterUserAsync(RegisterUserRequest request);

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="request">Запрос на авторизацию</param>
    /// <returns>Истинность авторизации</returns>
    Task<SessionDto> AuthUserAsync(AuthUserRequest request);
}