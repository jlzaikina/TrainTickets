using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class ScheduleMapper: IScheduleMapper
{
    public ScheduleDto Map(ScheduleEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new ScheduleDto()
        {
            NumberTrain = entity.Number_train,
            DepartureTime = entity.Date_departure.ToString("yyyy-MM-dd HH:mm"),
            ArrivalTime = entity.Date_arrival.ToString("yyyy-MM-dd HH:mm"),
            DepartureCityName = entity.Route.DepartureCity.Name,
            ArrivalCityName = entity.Route.ArrivalCity.Name,
        };
    }
}
