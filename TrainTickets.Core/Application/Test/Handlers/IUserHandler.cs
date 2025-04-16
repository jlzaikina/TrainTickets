using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <summary>
/// Работа с пользователем
/// </summary>
public interface IUserHandler
{
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

    /// <summary>
    /// Выход
    /// </summary>
    /// <param name="request">Запрос на выход</param>
    /// <returns>Истинность выхода</returns>
    Task<bool> LogoutUserAsync(LogoutRequest request);

    Task<bool> UpdateProfileAsync(string login, UpdateRequest request);

    Task<UserDto> GetUserDataToProfileAsync(string login);
}