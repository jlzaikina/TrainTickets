using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class TrainMapper : ITrainMapper
{
    public TrainDto Map(ScheduleEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return new TrainDto()
        {
            Number_train = entity.Number_train,
            DateDeparture = entity.Date_departure.ToString("dd.MM.yyyy HH:mm"),
            DateArrival = entity.Date_arrival.ToString("dd.MM.yyyy HH:mm"),
            Route = $"{entity.Route.DepartureCity.Name} — {entity.Route.ArrivalCity.Name}",
            TypeTrain = entity.Train.Type_train.Name,
            TravelDuration = entity.Date_arrival - entity.Date_departure,
            VanByType = entity.Train.Vans
                .GroupBy(v => v.Type_van.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => c.Number_van).OrderBy(n => n).ToList())
        };
    }

    public VanDto Map(VanEntity entity, List<int> seats)
    {
        return new VanDto()
        {
            CarriageSchemaJson = entity.Schema.Schema,
            OccupiedSeatNumbers = seats
        };
    }
}
