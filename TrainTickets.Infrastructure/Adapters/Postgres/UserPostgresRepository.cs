using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

/// <inheritdoc/>
public class UserPostgresRepository : IUserRepository
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
                if (pgEx.ConstraintName.Equals("user_unique_1"))
                {
                    throw new ApplicationException("Пользователь с таким email уже существует");
                }
                if (pgEx.ConstraintName.Equals("user_unique_2"))
                {
                    throw new ApplicationException("Пользователь с таким номером телефона уже существует");
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

    public async Task<SessionEntity> GetSessionByUserIdAsync(long id)
    {
        return await _dbContext.Sessions.FirstOrDefaultAsync(i => i.User_Id == id);
    }

    public async Task DeleteSession(string guid)
    {
        var session = await _dbContext.Sessions.FirstOrDefaultAsync(g => g.Guid == guid);

        if (session != null)
        {
            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task<UserEntity> GetPassengerByEmailAsync(string login)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }
    public async Task UpdateUser(UserEntity user)
    {
        try { 
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            if (pgEx.SqlState == "23505")
            {
                if (pgEx.ConstraintName.Equals("user_unique_1"))
                {
                    throw new ApplicationException("Пользователь с таким email уже существует");
                }
                if (pgEx.ConstraintName.Equals("user_unique_2"))
                {
                    throw new ApplicationException("Пользователь с таким номером телефона уже существует");
                }
            }
            throw;
        }
    }

    public async Task<PassengerEntity> GetPassengerByUserIdAsync(long id)
    {
        return await _dbContext.Passengers.FirstOrDefaultAsync(i => i.Id_passenger == id);
    }
    public async Task UpdatePassenger(PassengerEntity entity)
    {
        _dbContext.Passengers.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}
