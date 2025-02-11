using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

public interface IUserRepository
{
    /// <summary>
    /// Получить все.
    /// </summary>
    /// <returns><see cref="UserEntity"/></returns>
    Task<IEnumerable<UserEntity>> GetAllUserAsync();


    Task<UserEntity> GetUserByIdAsync(int id);
}