using TrainTickets.UI.Domain.Schedules;

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

    Task<IEnumerable<ScheduleDto>> GetFilterScheduleAsync(FilterRequest request);
}
