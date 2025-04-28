using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface ITrainHandler
{
    Task<TrainDto> GetInfoTrainInScheduleAsync(InfoTrainRequest request);
    Task<VanDto> GetShemaVanAsync(InfoVanRequest request);
    Task<double> GetPriceAsync(CheckRequest request);

    Task<TicketDto> CreateBookAsync(BookRequest request);
}
