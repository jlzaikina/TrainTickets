using Microsoft.EntityFrameworkCore;
using Npgsql;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

/// <inheritdoc/>
public class SchedulePostgresRepository: IScheduleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SchedulePostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ScheduleEntity>> GetSchedulesAsync()
    {
        return await _dbContext.Schedules
            .Include(s => s.Route)
                .ThenInclude(r => r.DepartureCity) // Включаем город отправления
            .Include(s => s.Route)
                .ThenInclude(r => r.ArrivalCity) // Включаем город прибытия
            .ToListAsync();
    }
    public async Task<IEnumerable<ScheduleEntity>> GetFilterScheduleAsync(FilterRequest request)
    {
        var query =  _dbContext.Schedules
            .Include(s => s.Route)
                .ThenInclude(r => r.DepartureCity) // Включаем город отправления
            .Include(s => s.Route)
                .ThenInclude(r => r.ArrivalCity) // Включаем город прибытия
            .AsQueryable();

        // Фильтр по городу прибытия
        if (!string.IsNullOrEmpty(request.ArrivalCityName))
        {
            query = query.Where(s => s.Route.ArrivalCity.Name == request.ArrivalCityName);
            var currentDate = DateTime.UtcNow.Date; // Текущая дата (без времени)
            query = query.Where(s => s.Date_departure.Date >= currentDate);
        }

        // Фильтр по дате отправления
        if (request.DepartureTime.HasValue)
        {
            query = query.Where(s => s.Date_departure.Date == request.DepartureTime.Value.Date);
        }

        // Фильтр по дате прибытия
        if (request.ArrivalTime.HasValue)
        {
            query = query.Where(s => s.Date_arrival.Date == request.ArrivalTime.Value.Date);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<CityEntity>> GetCitiesAsync()
    {
        return await _dbContext.Cities.ToListAsync();
    }

    public async Task DeleteTrip(InfoTrainRequest request)
    {
        var schedule =  await _dbContext.Schedules
            .Include(t => t.Bookings)
                .ThenInclude(b => b.Tickets)
            .FirstOrDefaultAsync(s => s.Number_train == request.Number_train &&
                                    s.Date_departure.Date == request.DateDeparture.Date);

        if (schedule != null)
        {
            _dbContext.Schedules.Remove(schedule);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ScheduleEntity> GetOneScheduleAsync(UpdateScheduleRequest request)
    {
        return await _dbContext.Schedules
           .Include(s => s.Route)
               .ThenInclude(r => r.DepartureCity)
           .Include(s => s.Route)
               .ThenInclude(r => r.ArrivalCity)
           .FirstOrDefaultAsync(s => s.Number_train == request.NumberTrain &&
                                    s.Date_departure.Date == request.DepartureTime.Date);
    }

    public async Task UpdateTrip(ScheduleEntity entity)
    {
        try
        {
            _dbContext.Schedules.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            throw;
        }
    }

    public async Task AddTrip(ScheduleEntity entity)
    {
        try
        {
            _dbContext.Schedules.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            throw;
        }
    }

    public async Task<RouteEntity> FindRouteAsync(int departureCityId, int arrivalCityId)
    {
       return await _dbContext.Routes
           .FirstOrDefaultAsync(r => r.City_departure == departureCityId &&
                                    r.City_arrival == arrivalCityId);
    }

    public async Task<CityEntity> GetCityByNameAsync(string cityName)
    {
        return await _dbContext.Cities
            .FirstOrDefaultAsync(c => c.Name == cityName);
    }

    public async Task<bool> ExistsScheduleAsync(int numberTrain, DateTime departureTime)
    {
        return await _dbContext.Schedules
          .Where(s => s.Number_train == numberTrain &&
                                   s.Date_departure.Date == departureTime.Date).AnyAsync();
    }

    public async Task<IEnumerable<RouteEntity>> GetRoutesAsync()
    {
        return await _dbContext.Routes
               .Include(r => r.DepartureCity)
               .Include(r => r.ArrivalCity)
           .ToListAsync();
    }
}
