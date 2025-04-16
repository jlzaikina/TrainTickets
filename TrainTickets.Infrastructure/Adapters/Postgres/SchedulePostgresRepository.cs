using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

/// <inheritdoc/>
public class SchedulePostgresRepository: IScheduleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SchedulePostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ScheduleEntity>> GetScheduleAsync()
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
}
