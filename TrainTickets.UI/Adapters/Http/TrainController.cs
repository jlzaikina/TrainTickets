using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Adapters.Http;

public class TrainController : ControllerBase
{
    private readonly ITrainHandler _trainHandler;
    public TrainController(ITrainHandler trainHandler)
    {
        _trainHandler = trainHandler ?? throw new ArgumentNullException(nameof(trainHandler));
    }

    /// <summary>
    /// Получить расписание
    /// </summary>
    /// <returns><see cref="TrainDto"/></returns>
    /// 
    [HttpPost]
    [Route("/api/v1/train/get-info")]
    public async Task<TrainDto> GetInfoTrainInSchedule([FromBody] InfoTrainRequest request)
    {
        var ent = _trainHandler.GetInfoTrainInScheduleAsync(request);
        return await ent;
    }
}
