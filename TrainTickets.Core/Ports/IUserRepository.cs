using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

/// <summary>
/// Работа с пользователем в бд
/// </summary>

public interface IUserRepository
{
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns><see cref="UserEntity"/></returns>
    Task<IEnumerable<UserEntity>> GetAllUserAsync();

    /// <summary>
    /// Получить пользователя по ID
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns><see cref="UserEntity"/></returns>
    Task<UserEntity> GetUserByIdAsync(int id);

    /// <summary>
    /// Добавить нового пользователя
    /// </summary>
    /// <param name="entity">Пользователь</param>
    /// <returns><see cref="UserEntity"/></returns>
    UserEntity AddUser(UserEntity entity);

    /// <summary>
    /// Получить пользователя по логину
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns><see cref="UserEntity"/></returns>
    Task<UserEntity> GetUserByLoginAsync(string login);

}