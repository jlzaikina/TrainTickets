using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface ITrainMapper
{
    TrainDto Map(ScheduleEntity entity);
}
