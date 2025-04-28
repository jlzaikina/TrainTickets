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

    /// <summary>
    /// Получить номера поездов
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-van-number")]
    public async Task<IEnumerable<int>> GetVanNumber()
    {
        return await _trainHandler.GetVanNumberAsync();
    }

    /// <summary>
    /// Получить все схемы
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-all-schema")]
    public async Task<IEnumerable<SchemaDto>> GetAllSchema()
    {
        return await _trainHandler.GetAllSchemaAsync();
    }

    /// <summary>
    /// Получить поезда
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-schema/{schemaId}")]
    public async Task<SchemaDto> GetSchema(int schemaId)
    {

        return await _trainHandler.GetSchemaAsync(schemaId);

    }
    /// <summary>
    /// Сохранить новую схему
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/save-schema")]
    public async Task<ActionResult<bool>> SaveSchema([FromBody] SaveSchemaRequest request)
    {
        try
        {
            return await _trainHandler.SaveSchemaAsync(request);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Изменить схему
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPut]
    [Route("/api/v1/train/update-schema/{id}")]
    public async Task<ActionResult<bool>> UpdateSchema(int id, [FromBody] SaveSchemaRequest request)
    {
        try
        {
            return await _trainHandler.UpdateSchemaAsync(id, request);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Изменить схему
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/delete-schema/{id}")]
    public async Task<ActionResult<bool>> DeleteSchema(int id)
    {
        try
        {
            return await _trainHandler.DeleteSchemaAsync(id);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Создать поезд
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/create-train")]
    public async Task<ActionResult<bool>> CreateTrain([FromBody] CreateTrainRequest request)
    {
        try
        {
            return await _trainHandler.CreateTrainAsync(request);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Удалить поезд
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/delete-train/{numberTrain}")]
    public async Task<ActionResult<bool>> DeleteTrain(int numberTrain)
    {
        try
        {
            return await _trainHandler.DeleteTrainAsync(numberTrain);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Получить поезда
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-trains")]
    public async Task<IEnumerable<TrainDetailsDto>> GetTrains()
    {
        
        return await _trainHandler.GetTrainsAsync();
        
    }

    /// <summary>
    /// Изменить поезд
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpPut]
    [Route("/api/v1/train/update-train")]
    public async Task<ActionResult<bool>> UpdateTrain([FromBody] TrainDetailsDto request)
    {

        return await _trainHandler.UpdateTrainAsync(request);

    }

    /// <summary>
    /// Получить типы поездов
    /// </summary>
    /// <returns><see cref="ScheduleDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-type-trains")]
    public async Task<IEnumerable<string>> GetTypeTrains()
    {

        return await _trainHandler.GetTypeTrainsAsync();

    }
}
