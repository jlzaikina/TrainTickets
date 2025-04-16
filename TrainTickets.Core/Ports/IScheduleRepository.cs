using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

/// <summary>
/// Работа с расписанием в бд
/// </summary>
public interface IScheduleRepository
{
    /// <summary>
    /// Получить расписание
    /// </summary>
    /// <returns><see cref="ScheduleEntity"/></returns>
    Task<IEnumerable<ScheduleEntity>> GetScheduleAsync();

    Task<IEnumerable<ScheduleEntity>> GetFilterScheduleAsync(FilterRequest request);
}

