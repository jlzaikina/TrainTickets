using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
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
    Task<IEnumerable<ScheduleEntity>> GetSchedulesAsync();
    Task<ScheduleEntity>  GetScheduleAsync(InfoTrainRequest request);
    Task<ScheduleEntity> GetOneScheduleAsync(UpdateScheduleRequest request);
    Task<RouteEntity> FindRouteAsync(int departureCityId, int arrivalCityId);
    Task<CityEntity> GetCityByNameAsync(string cityName);
    Task<bool> ExistsScheduleAsync(int numberTrain, DateTime departureTime);
    Task DeleteTrip(InfoTrainRequest request);
    Task AddTrip(ScheduleEntity entity);
    Task UpdateTrip(ScheduleEntity entity);
    Task<IEnumerable<CityEntity>> GetCitiesAsync();
    Task<IEnumerable<ScheduleEntity>> GetFilterScheduleAsync(FilterRequest request);

    Task<IEnumerable<RouteEntity>> GetRoutesAsync();
}

