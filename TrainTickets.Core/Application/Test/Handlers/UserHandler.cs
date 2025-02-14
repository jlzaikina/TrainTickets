using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

// тут пишется логика
public class UserHandler : IUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserHandler(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
    }

    public async Task<IEnumerable<UserDto>> GetAllUserAsync()
    {
        var userEntity = await _userRepository.GetAllUserAsync();

        //List<TestDto> list = new List<TestDto>();
        //foreach (var i in testEntity)
        //{
        //    list.Add(new TestDto { Id = i.Id});
        //}

        return userEntity.Select(_userMapper.Map);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var userEntity = await _userRepository.GetUserByIdAsync(id);

        //List<TestDto> list = new List<TestDto>();
        //foreach (var i in testEntity)
        //{
        //    list.Add(new TestDto { Id = i.Id});
        //}

        return _userMapper.Map(userEntity);
    }


    public async Task<bool> RegisterUserAsync(RegisterUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Пользователь уже существует");
        }

        var result = _userMapper.Map(_userRepository.AddUser(_userMapper.Map(request)));
        if (result.Id == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> AuthUserAsync(AuthUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser == null || existingUser.Password != request.Password)
        {
            throw new InvalidOperationException("Неверный логин или пароль");
        }
        return true;
    }
}