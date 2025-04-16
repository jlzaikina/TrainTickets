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

    /// <summary>
    /// Добавить сессию пользователя
    /// </summary>
    /// <param name="entity">Сессия</param>
    /// <returns><see cref="SessionEntity"/></returns>
    SessionEntity AddSession(SessionEntity entity);

    /// <summary>
    /// Получить сессию по ID пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns><see cref="SessionEntity"/></returns>
    Task<SessionEntity> GetSessionByUserIdAsync(long id);

    /// <summary>
    /// ПУдалить сессию
    /// </summary>
    /// <param name="guid">Guid</param>
    Task DeleteSession(string guid);
    Task UpdateUser(UserEntity user);

    Task<PassengerEntity> GetPassengerByUserIdAsync(long id);
    Task UpdatePassenger(PassengerEntity entity);
}