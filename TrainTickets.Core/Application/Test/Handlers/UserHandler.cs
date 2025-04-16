using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Xml.Linq;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <inheritdoc/>
public class UserHandler : IUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPassRepository _passRepository;
    private readonly IUserMapper _userMapper;
    private readonly IPassMapper _passMapper;

    private const string ADMIN_LOGIN = "admin";
    private const string ADMIN_PASSWORD = "admin";


    public UserHandler(IUserRepository userRepository, IPassRepository passRepository, IUserMapper userMapper, IPassMapper passMapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passRepository = passRepository ?? throw new ArgumentNullException(nameof(passRepository));
        _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        _passMapper = passMapper ?? throw new ArgumentNullException(nameof(passMapper));
    }

    public async Task<bool> RegisterUserAsync(RegisterUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser != null)
        {
            throw new ApplicationException("Пользователь уже существует");
        }
       
        if (request.Login.Length > 30 || request.Login.Length < 3)
        {
            throw new ApplicationException("Логин от 3 до 30 символов");
        }
        if (request.Password.Length > 30 || request.Password.Length < 8)
        {
            throw new ApplicationException("Пароль от 8 до 30 символов");
        }
        if (request.Email.Length > 50 || request.Email.Length < 6)
        {
            throw new ApplicationException("Email от 6 до 50 символов");
        }
        if (request.Surname.Length > 50 || request.Surname.Length < 2)
        {
            throw new ApplicationException("Фамилия от 2 до 50 символов");
        }
        if (request.Name.Length > 50 || request.Name.Length < 2)
        {
            throw new ApplicationException("Имя от 2 до 50 символов");
        }
        if (request.Midname?.Length > 50)
        {
            throw new ApplicationException("Отчество до 50 символов");
        }
        if (request.Phone.Length != 11)
        {
            throw new ApplicationException("Номер телефона 11-значное число");
        }

        var addUser = _userRepository.AddUser(_userMapper.Map(request));

        _passRepository.AddPassenger(_passMapper.Map(addUser));

        return true;
    }

    public async Task<SessionDto> AuthUserAsync(AuthUserRequest request)
    {
        if (ADMIN_LOGIN == request.Login && ADMIN_PASSWORD == request.Password)
        {
            var sessionAdmin = new SessionEntity
            {
                Guid = Guid.NewGuid().ToString()
            };
            return new SessionDto
            {
                Guid = sessionAdmin.Guid
            };
        }
        var existingUser = await _userRepository.GetUserByLoginAsync(request.Login);
        if (existingUser == null || existingUser.Password != request.Password)
        {
            throw new ApplicationException("Неверный логин или пароль");
        }
        var session = await _userRepository.GetSessionByUserIdAsync(existingUser.Id);
        if (session == null)
        {
            var result = _userMapper.Map(_userRepository.AddSession(_userMapper.Map1(existingUser)));
            return result;
        }
        return _userMapper.Map(session);

    }

    public async Task<bool> LogoutUserAsync(LogoutRequest request)
    {
        await _userRepository.DeleteSession(request.Guid);
        return true;
    }
    public async Task<bool> UpdateProfileAsync(string login, UpdateRequest request)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);

        var pass = await _passRepository.GetPassengerByEmailAsync(user.Id,user.Email);

        var age = DateTime.Now.Year - request.Date_birth?.Year;
        if (DateTime.Now.Month < request.Date_birth?.Month ||
            (DateTime.Now.Month == request.Date_birth?.Month && DateTime.Now.Day < request.Date_birth?.Day)) age--;


        if ((age < 14) || (age > 110))
        {
            throw new ApplicationException("Проверьте дату рождения на корректность");
        }

        if (request.Passport?.Length < 10)
        {
            throw new ApplicationException("Серия и номер паспорта 10-значное число");
        }
        if (request.Email.Length > 50 || request.Email.Length < 6)
        {
            throw new ApplicationException("Email от 6 до 50 символов");
        }
        if (request.Surname == null || request.Surname.Length > 50 || request.Surname.Length < 2)
        {
            throw new ApplicationException("Фамилия от 2 до 50 символов");
        }
        if (request.Name.Length > 50 || request.Name.Length < 2)
        {
            throw new ApplicationException("Имя от 2 до 50 символов");
        }
        if (request.Midname?.Length > 50)
        {
            throw new ApplicationException("Отчество до 50 символов");
        }
        if (request.Phone.Length != 11)
        {
            throw new ApplicationException("Номер телефона 11-значное число");
        }

        if (pass.Email != request.Email &&
        !await _passRepository.IsEmailUniqueForPassAsync(user.Id, request.Email))
        {
            throw new ApplicationException("Пользователь с таким email уже есть");
        }
        if (pass.Passport != request.Passport &&
        !await _passRepository.IsPassportUniqueGlobalAsync(request.Passport, pass.Id_user))
        {
            throw new ApplicationException("Пользователь с таким паспортом уже есть");
        }
        if (!await _passRepository.IsPassportUniqueForUserAsync(request.Passport, pass.Id_user, pass.Id_passenger))
            throw new ApplicationException("У вас уже есть с таким паспортом пассажир");

        user.Email = request.Email;
        user.Phone = request.Phone;
        user.Surname = request.Surname;
        user.Name = request.Name;
        user.Midname = request.Midname;


        pass.Email = request.Email;
        pass.Surname = request.Surname;
        pass.Name = request.Name;
        pass.Midname = request.Midname;
        pass.Passport = request.Passport;
        pass.Date_birth = request.Date_birth;


        await _userRepository.UpdateUser(user);

        //await _userRepository.UpdatePassenger(pass);


        return true;
    }
    public async Task<UserDto> GetUserDataToProfileAsync(string login)
    {
        var userEntity = await _userRepository.GetUserByLoginAsync(login);
        var passEntity = await _passRepository.GetPassengerByEmailAsync(userEntity.Id, userEntity.Email);  
        return _userMapper.Map(userEntity, passEntity);
    }
}