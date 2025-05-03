using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Adapters.Http;

public class ScheduleController: ControllerBase
{
    private readonly IScheduleHandler _scheduleHandler;
    public ScheduleController(IScheduleHandler scheduleHandler)
    {
        _scheduleHandler = scheduleHandler ?? throw new ArgumentNullException(nameof(scheduleHandler));
    }

    /// <summary>
    /// Получить расписание
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/shedule/get-all")]
    public async Task<IEnumerable<ScheduleDto>> GetSchedule()
    {
        return await _scheduleHandler.GetScheduleAsync();
    }

    /// <summary>
    ///Фильтрация расписания
    /// </summary>
    /// <param><see cref="FilterRequest"/></param>
    /// <returns>Истинность вывода</returns>
    [HttpPost]
    [Route("/api/v1/shedule/get-filter")]
    public async Task<ActionResult<bool>> GetFilterSchedule([FromBody] FilterRequest request)
    {
        try
        {
            var result = await _scheduleHandler.GetFilterScheduleAsync(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка вывода");
        }
    }

    /// <summary>
    /// Получить список городов
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/shedule/get-city")]
    public async Task<IEnumerable<CityDto>> GetCities()
    {
        return await _scheduleHandler.GetCitiesAsync();
    }

    /// <summary>
    /// Удаление рейса
    /// </summary>
    /// <param><see cref="LogoutRequest"/></param>
    /// <returns>Истинность выхода</returns>
    [HttpPost]
    [Route("/api/v1/shedule/delete")]
    public async Task<ActionResult<bool>> DeleteTrip([FromBody] InfoTrainRequest request)
    {
        try
        {
            var result = await _scheduleHandler.DeleteTripAsync(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка удаления");
        }
    }

    /// <summary>
    /// Редактирование рейса
    /// </summary>
    /// <param><see cref="LogoutRequest"/></param>
    /// <returns>Истинность выхода</returns>
    [HttpPut]
    [Route("/api/v1/shedule/update")]
    public async Task<ActionResult<bool>> UpdateTrip([FromBody] UpdateScheduleRequest request)
    {
        try
        {
            var result = await _scheduleHandler.UpdateTripAsync(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка обновления");
        }
    }

    [HttpPost]
    [Route("/api/v1/shedule/create")]
    public async Task<ActionResult<bool>> CreateTrip([FromBody] CreateScheduleRequest request)
    {
        try
        {
            var result = await _scheduleHandler.CreateTripAsync(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка добавления");
        }
    }

    [HttpGet]
    [Route("/api/v1/shedule/get-routes")]
    public async Task<IEnumerable<RouteDto>> GetRoutes()
    {
        return await _scheduleHandler.GetRoutesAsync();
    }
}
