using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <inheritdoc/>
public class UserHandler : IUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly ISessionRepository _sessionRepository;

    public UserHandler(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
    }

    public async Task<IEnumerable<UserDto>> GetAllUserAsync()
    {
        var userEntity = await _userRepository.GetAllUserAsync();
        return userEntity.Select(_userMapper.Map);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var userEntity = await _userRepository.GetUserByIdAsync(id);
        return _userMapper.Map(userEntity);
    }


    public async Task<bool> RegisterUserAsync(RegisterUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser != null)
        {
            throw new ApplicationException("Пользователь уже существует");
        }

        var result = _userMapper.Map(_userRepository.AddUser(_userMapper.Map(request)));
        if (result.Id == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<SessionDto> AuthUserAsync(AuthUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser == null || existingUser.Password != request.Password)
        {
            throw new ApplicationException("Неверный логин или пароль");
        }

        var userEntity = _userMapper.Map1(existingUser);

        var result = _sessionRepository.AddSession(userEntity);

        var result1 = _userMapper.Map(result);

        return result1;
    }
}