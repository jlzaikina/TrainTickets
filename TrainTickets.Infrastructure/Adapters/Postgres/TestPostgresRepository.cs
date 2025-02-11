using Microsoft.EntityFrameworkCore;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

public class TestPostgresRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TestPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<UserEntity> GetUserByIdAsync(int id)
    {
        string sqlQuery = "SELECT * FROM \"User\" WHERE \"Id\" = {0}";
        return await _dbContext.Users.FromSqlRaw(sqlQuery, id).FirstOrDefaultAsync();
    }
}
