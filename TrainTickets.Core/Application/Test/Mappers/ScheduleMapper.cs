using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Entities;
using static iTextSharp.text.pdf.events.IndexEvents;

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

    public CityDto Map(CityEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new CityDto()
        {
            City = entity.Name
        };
    }

    public ScheduleEntity Map(CreateScheduleRequest request, int id)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new ScheduleEntity()
        {
            Number_train = request.NumberTrainNew,
            Date_departure = request.DepartureTimeNew,
            Date_arrival = request.ArrivalTimeNew,
            Id_route = id
        };
    }

    public RouteDto Map(RouteEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new RouteDto()
        {
            Route = $"{entity.DepartureCity.Name}-{entity.ArrivalCity.Name}"
        };
    }
}
