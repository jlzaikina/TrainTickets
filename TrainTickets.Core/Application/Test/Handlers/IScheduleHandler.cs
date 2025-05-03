using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <summary>
/// Работа с расписанием
/// </summary>
public interface IScheduleHandler
{
    /// <summary>
    /// Получить расписание.
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    Task<IEnumerable<ScheduleDto>> GetScheduleAsync();

    Task<IEnumerable<CityDto>> GetCitiesAsync();
    Task<bool> DeleteTripAsync(InfoTrainRequest request);
    Task<bool> UpdateTripAsync(UpdateScheduleRequest request);
    Task<bool> CreateTripAsync(CreateScheduleRequest request);
    Task<IEnumerable<ScheduleDto>> GetFilterScheduleAsync(FilterRequest request);
    Task<IEnumerable<RouteDto>> GetRoutesAsync();
}
