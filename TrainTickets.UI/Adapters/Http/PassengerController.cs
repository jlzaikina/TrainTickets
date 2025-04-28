using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.Passenger;
using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Adapters.Http;

public class PassengerController: ControllerBase
{
    private readonly IPassengerHandler _passHandler;

    public PassengerController(IPassengerHandler passHandler)
    {
        _passHandler = passHandler ?? throw new ArgumentNullException(nameof(passHandler));
    }

    /// <summary>
    /// Добавление пассажира
    /// </summary>
    /// <param><see cref="CreatePassRequest"/></param>
    /// <returns>Истинность добавления</returns>
    [HttpPost]
    [Route("/api/v1/pass/create/{login}")]
    public async Task<ActionResult<bool>> CreatePass(string login, [FromBody] CreatePassRequest request)
    {
        try
        {
            var result = await _passHandler.CreatePassAsync(login, request);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка добавления пассажира");
        }
    }
    /// <summary>
    /// Удаление пассажира
    /// </summary>
    /// <param><see cref="DeletePassRequest"/></param>
    /// <returns>Истинность удаления</returns>
    [HttpPost]
    [Route("/api/v1/pass/delete/{login}")]
    public async Task<ActionResult<bool>> DeletePassenger(string login, [FromBody] DeletePassRequest request)
    {
        try
        {
            var result = await _passHandler.DeletePassengerAsync(login, request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка удаления пассажира");
        }
    }

    /// <summary>
    /// Получение пассажиров пользователя
    /// </summary>
    /// < returns >Данные пассажира</returns>
    [HttpGet]
    [Route("/api/v1/pass/get-all-pass/{login}")]
    public async Task<IEnumerable<PassengerDto>> GetPassengerData(string login)
    {
        return await _passHandler.GetPassengerDataAsync(login);
    }
    /// <summary>
    /// Получение пассажиров пользователя
    /// </summary>
    /// < returns >Данные пассажира</returns>
    [HttpGet]
    [Route("/api/v1/pass/get-pass-book/{login}")]
    public async Task<IEnumerable<PassengerDto>> GetPassengerDataForBook(string login)
    {
        return await _passHandler.GetPassengerDataForBookAsync(login);
    }

    /// <summary>
    /// Удаление пассажира
    /// </summary>
    /// <param><see cref="DeletePassRequest"/></param>
    /// <returns>Истинность удаления</returns>
    [HttpGet]
    [Route("/api/v1/pass/is-self/{login}/{passengerId}")]
    public async Task<ActionResult<bool>> IsSelfPassenger(string login, long passengerId)
    {
        
            var result = await _passHandler.IsSelfPassengerAsync(login, passengerId);
            return Ok(result);        
    }
}
