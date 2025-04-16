using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.Schedules;
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
}
