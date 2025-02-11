using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Domain;

namespace TrainTickets.UI.Adapters.Http;

public class UserController : ControllerBase
{
    private readonly IUserHandler _testHandler;

    public UserController(IUserHandler testHandler)
    {
        _testHandler = testHandler ?? throw new ArgumentNullException(nameof(testHandler));
    }
    
    /// <summary>
    /// Получить список.
    /// </summary>
    [HttpGet]
    [Route("/api/v1/user/get-all")]
    public async Task<IEnumerable<UserDto>> GetAllUser()
    {
        return await _testHandler.GetAllUserAsync();
    }

    [HttpGet]
    [Route("/api/v1/user/get-id/{id}")]
    public async Task<UserDto> GetUserById(int id)
    {
        return await _testHandler.GetUserByIdAsync(id);
    }
}