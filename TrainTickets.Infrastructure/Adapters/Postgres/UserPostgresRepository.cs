using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

/// <inheritdoc/>
public class UserPostgresRepository : IUserRepository, ISessionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<UserEntity> GetUserByIdAsync(int id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<UserEntity> GetUserByLoginAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public UserEntity AddUser(UserEntity entity)
    {
        try
        {
            var addEntity = _dbContext.Users.Add(entity).Entity;
            _dbContext.SaveChanges();
            return addEntity;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            if (pgEx.SqlState == "23505")
            {
                if (pgEx.ConstraintName.Contains("user_unique_1"))
                {
                    throw new ApplicationException("Пользователь с таким email уже существует");
                }
                if (pgEx.ConstraintName.Contains("user_unique_2"))
                {
                    throw new ApplicationException("Пользователь с таким номером телефона уже существует");
                }
            }
            if (pgEx.SqlState == "23514")
            {
                if (pgEx.ConstraintName.Contains("user_check1"))
                {
                    throw new ApplicationException("Логин от 4 до 8 символов");
                }
            }
            throw;
        }
    }
    public SessionEntity AddSession(SessionEntity session)
    {
        var addEntity = _dbContext.Sessions.Add(session).Entity;
        _dbContext.SaveChanges();
        return addEntity;
    }
}
