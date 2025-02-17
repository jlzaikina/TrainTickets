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
    /// Получить список пользователей.
    /// </summary>
    /// <returns><see cref="UserDto"/></returns>
    [HttpGet]
    [Route("/api/v1/user/get-all")]
    public async Task<IEnumerable<UserDto>> GetAllUser()
    {
        return await _userHandler.GetAllUserAsync();
    }


    /// <summary>
    /// Получить пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns><see cref="UserDto"/></returns>
    [HttpGet]
    [Route("/api/v1/user/get-id/{id}")]
    public async Task<UserDto> GetUserById([FromRoute] int id)
    {
        return await _userHandler.GetUserByIdAsync(id);
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param><see cref="RegisterUserRequest"/></param>
    /// <returns>Истинность регистрации</returns>
    [HttpPost]
    [Route("/api/v1/user/create")]
    public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterUserRequest request)
    {
        try
        {
            var result = await _userHandler.RegisterUserAsync(request);
            return Ok(result);
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


    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param><see cref="AuthUserRequest"/></param>
    /// <returns>Истинность авторизации</returns>
    [HttpPost]
    [Route("/api/v1/user/auth")]
    public async Task<IActionResult> AuthUser([FromBody] AuthUserRequest request)
    {
        try
        {
            var session = await _userHandler.AuthUserAsync(request);

            return Ok(new {Token = session.Guid, Username = request.Login });
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка авторизации");
        }
    }
}