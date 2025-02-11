using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Internal;

// тут только работа с БД
public class TestRepository : IUserRepository
{
    private readonly List<UserEntity> _tests = new() {new UserEntity {Id = 1}};
    
    public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
    {
        return await Task.FromResult(_tests);
    }

    public Task<UserEntity> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}