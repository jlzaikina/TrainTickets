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
    /// Получение данных о поезде
    /// </summary>
    /// <param><see cref="InfoTrainRequest"/></param>
    /// <returns><see cref="TrainDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/get-info")]
    public async Task<TrainDto> GetInfoTrainInSchedule([FromBody] InfoTrainRequest request)
    {
        var entity = _trainHandler.GetInfoTrainInScheduleAsync(request);
        return await entity;
    }

    /// <summary>
    /// Получение данных для бронирования места
    /// </summary>
    /// <param><see cref="InfoTrainRequest"/></param>
    /// <returns><see cref="VanDto"/></returns>
    [HttpPost]
    [Route("/api/v1/train/get-shema")]
    public async Task<VanDto> GetShemaVan([FromBody] InfoVanRequest request)
    {
        var ent = _trainHandler.GetShemaVanAsync(request);
        return await ent;
    }

    /// <summary>
    /// Получение стоимости места
    /// </summary>
    /// <param><see cref="CheckRequest"/></param>
    /// <returns>Стоимость</returns>
    [HttpPost]
    [Route("/api/v1/train/get-price")]
    public async Task<double> GetPrice([FromBody] CheckRequest request)
    {
        var ent = _trainHandler.GetPriceAsync(request);
        return await ent;
    }

    /// <summary>
    /// Создание брони и билета
    /// </summary>
    /// <param><see cref="BookRequest"/></param>
    /// <returns><see cref="TicketDto"/></returns>
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
    /// Получение номеров поездов
    /// </summary>
    /// <returns>Список номеров</returns>
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
    /// Получить схему
    /// </summary>
    /// <returns><see cref="SchemaDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-schema/{schemaId}")]
    public async Task<SchemaDto> GetSchema(int schemaId)
    {

        return await _trainHandler.GetSchemaAsync(schemaId);

    }

    /// <summary>
    /// Добавить схему
    /// </summary>
    /// <param><see cref="SaveSchemaRequest"/></param>
    /// <returns>Истинность выполнения</returns>
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
    /// Изменение схемы
    /// </summary>
    /// <param><see cref="SaveSchemaRequest"/></param>
    /// <returns>Истинность выполнения</returns>
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
    /// Удалить схему
    /// </summary>
    /// <param>Ид схемы</param>
    /// <returns><see cref="TicketDto"/></returns>
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
    /// Добавить поезд
    /// </summary>
    /// <param><see cref="CreateTrainRequest"/></param>
    /// <returns>Истинность добавления</returns>
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
    /// <param>Номер поезда</param>
    /// <returns>Истинность удаления</returns>
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
    /// Получение поездов
    /// </summary>
    /// <returns><see cref="TrainDetailsDto"/></returns>
    [HttpGet]
    [Route("/api/v1/train/get-trains")]
    public async Task<IEnumerable<TrainDetailsDto>> GetTrains()
    {
        
        return await _trainHandler.GetTrainsAsync();
        
    }

    /// <summary>
    /// Изменение поезда
    /// </summary>
    /// <param><see cref="TrainDetailsDto"/></param>
    /// <returns>Истинность выполнения</returns>
    [HttpPut]
    [Route("/api/v1/train/update-train")]
    public async Task<ActionResult<bool>> UpdateTrain([FromBody] TrainDetailsDto request)
    {

        return await _trainHandler.UpdateTrainAsync(request);

    }

    /// <summary>
    /// Получить типы поездов
    /// </summary>
    /// <returns>Список названий</returns>
    [HttpGet]
    [Route("/api/v1/train/get-type-trains")]
    public async Task<IEnumerable<string>> GetTypeTrains()
    {

        return await _trainHandler.GetTypeTrainsAsync();

    }

    /// <summary>
    /// Проверка перед изменением поезда
    /// </summary>
    [HttpPost]
    [Route("/api/v1/train/check-update")]
    public async Task<ActionResult<bool>> CheckUpdateTrain(int numberTrain)
    {

        return await _trainHandler.CheckUpdateTrainAsync(numberTrain);

    }

    /// <summary>
    /// Проверка перед изменением схемы
    /// </summary>
    [HttpPost]
    [Route("/api/v1/train/check-update-schema/{schemaId}")]
    public async Task<ActionResult<bool>> CheckUpdateSchema(int schemaId)
    {

        return await _trainHandler.CheckUpdateSchemaAsync(schemaId);

    }
}
