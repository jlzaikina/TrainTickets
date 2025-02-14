using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain.User;

namespace TrainTickets.UI.Adapters.Http;

public class UserController : ControllerBase
{
    private readonly IUserHandler _userHandler;

    public UserController(IUserHandler userHandler)
    {
        _userHandler = userHandler ?? throw new ArgumentNullException(nameof(userHandler));
    }

    /// <summary>
    /// Получить список.
    /// </summary>
    [HttpGet]
    [Route("/api/v1/user/get-all")]
    public async Task<IEnumerable<UserDto>> GetAllUser()   //получаем всех
    {
        return await _userHandler.GetAllUserAsync();
    }

    [HttpGet]
    [Route("/api/v1/user/get-id/{id}")]
    public async Task<UserDto> GetUserById([FromRoute] int id)       //получаем по id
    {
        return await _userHandler.GetUserByIdAsync(id);
    }

    [HttpPost]
    [Route("/api/v1/user/create")]
    public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterUserRequest request)   //регистрируем
    {
        try
        {
            var result = await _userHandler.RegisterUserAsync(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка регистрации");
        }
    }

    [HttpPost]
    [Route("/api/v1/user/auth")]

    public async Task<ActionResult<bool>> AuthUser([FromBody] AuthUserRequest request)   //авторизируем
    {
        try
        {
            var result = await _userHandler.AuthUserAsync(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка авторизации");
        }
    }
}