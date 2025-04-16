using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

public class TrainPostgresRepository : ITrainRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TrainPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ScheduleEntity> GetInfoTrainInScheduleAsync(InfoTrainRequest request)
    {
       return await _dbContext.Schedules
            .Include(s => s.Route)
                .ThenInclude(r => r.DepartureCity)
            .Include(s => s.Route)
                .ThenInclude(r => r.ArrivalCity)
            .Include(s => s.Train)
                .ThenInclude(t => t.Type_train)
            .Include(s => s.Train)
                .ThenInclude(t => t.Vans)
                    .ThenInclude(v => v.Type_van)
            .FirstOrDefaultAsync(t => t.Number_train == request.Number_train && t.Date_departure.Date == request.DateDeparture.Date);


    }
}
