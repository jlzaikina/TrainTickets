using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Entities;


namespace TrainTickets.UI.Application.Test.Mappers;

public interface IScheduleMapper
{
    ScheduleDto Map(ScheduleEntity entity);

    ScheduleEntity Map(CreateScheduleRequest request, int id);

    CityDto Map(CityEntity entity);

    RouteDto Map(RouteEntity entity);
}
