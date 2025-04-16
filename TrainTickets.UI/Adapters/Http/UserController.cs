using Microsoft.AspNetCore.Identity.Data;
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
    public async Task<ActionResult> AuthUser([FromBody] AuthUserRequest request)
    {
        try
        {
            var session = await _userHandler.AuthUserAsync(request);

            return Ok(new { Token = session.Guid, Username = request.Login });
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

    /// <summary>
    /// Выход пользователя
    /// </summary>
    /// <param><see cref="LogoutRequest"/></param>
    /// <returns>Истинность выхода</returns>
    [HttpPost]
    [Route("/api/v1/user/logout")]
    public async Task<ActionResult<bool>> LogoutUser([FromBody] LogoutRequest request)
    {
        try
        {
            var result = await _userHandler.LogoutUserAsync(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка выхода");
        }
    }
    /// <summary>
    /// Получение данных для профиля по логину
    /// </summary>
    /// < returns >Данные пользователя</returns>
    [HttpGet]
    [Route("/api/v1/user/get-user/{login}")]
    public async Task<UserDto> GetUserDataToProfile(string login)
    {

        return await _userHandler.GetUserDataToProfileAsync(login);
    }

    /// <summary>
    /// Обновление полей профиля
    /// </summary>
    /// <param><see cref = "UpdateRequest" /></ param >
    /// < returns > Истинность обновления</returns>
    [HttpPut]
    [Route("/api/v1/user/update/{login}")]
    public async Task<ActionResult<bool>> UpdateProfile(string login, [FromBody] UpdateRequest request)
    {
        try
        {
            var result = await _userHandler.UpdateProfileAsync(login, request);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка выхода");
        }
    }
}