using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Ticket;
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
        var entity = _trainHandler.GetInfoTrainInScheduleAsync(request);
        return await entity;
    }

    [HttpPost]
    [Route("/api/v1/train/get-shema")]
    public async Task<VanDto> GetShemaVan([FromBody] InfoVanRequest request)
    {
        var ent = _trainHandler.GetShemaVanAsync(request);
        return await ent;
    }

    [HttpPost]
    [Route("/api/v1/train/get-price")]
    public async Task<double> GetPrice([FromBody] CheckRequest request)
    {
        var ent = _trainHandler.GetPriceAsync(request);
        return await ent;
    }

    [HttpPost]
    [Route("/api/v1/train/create-book")]
    public async Task<ActionResult<TicketDto>> CreateBooking([FromBody] BookRequest request)
    {
        try
        {
            var ent = _trainHandler.CreateBookAsync(request);
            return await ent;
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
