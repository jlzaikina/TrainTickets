using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

public class PassPostgresRepository: IPassRepository
{
    private readonly ApplicationDbContext _dbContext;
    public PassPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PassengerEntity> GetPassengerByEmailAsync(long id, string email)
    {
        return await _dbContext.Passengers.FirstOrDefaultAsync(p => p.Id_user == id && p.Email == email);
    }
    public async Task<PassengerEntity> GetPassengerByPassportAsync(long id, string passport)
    {
        return await _dbContext.Passengers.FirstOrDefaultAsync(p => p.Id_user == id && p.Passport == passport);
    }
    public async Task AddPassenger(PassengerEntity entity)
    {
        try
        {
            _dbContext.Passengers.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            
            throw;
        }
    }
    public async Task DeletePassenger(long id, long id_pass)
    {
        var pass = await _dbContext.Passengers.FirstOrDefaultAsync(i => i.Id_user == id && i.Id_passenger == id_pass);

        if (pass != null)
        {
            _dbContext.Passengers.Remove(pass);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<PassengerEntity>> GetPassengerAsync(long id, string email)
    {
        return await _dbContext.Passengers
                .Where(p => p.Id_user == id) // Фильтруем по UserId
                .Where(p => !(p.Email == email)) // Исключаем пассажира, который соответствует пользователю
                .ToListAsync();
    }
    public async Task<bool> IsEmailUniqueForPassAsync(long id, string email)
    {
        var query = _dbContext.Passengers
                .Where(p => p.Id_user == id && p.Email == email);

        return !await query.AnyAsync();
    }

    public async Task<bool> IsPassportUniqueGlobalAsync(string passportNumber, long? excludeUserId = null)
    {

        var query = _dbContext.Passengers.Where(p => p.Passport == passportNumber &&
                       p.Is_self);

        if (excludeUserId.HasValue)
            query = query.Where(p => p.Id_user != excludeUserId.Value);

        return !await query.AnyAsync();
    }
    public async Task<bool> IsPassportUniqueForUserAsync(string passportNumber, long userId, long? excludePassengerId = null)
    {
        var query = _dbContext.Passengers
            .Where(p => p.Id_user == userId && p.Passport == passportNumber &&
                       !p.Is_self);

        if (excludePassengerId.HasValue)
            query = query.Where(p => p.Id_passenger != excludePassengerId.Value);

        return !await query.AnyAsync();
    }
}
