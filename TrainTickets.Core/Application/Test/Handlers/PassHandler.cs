using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Passenger;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

public class PassHandler: IPassengerHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPassRepository _passRepository;
    private readonly IUserMapper _userMapper;
    private readonly IPassMapper _passMapper;

    public PassHandler(IUserRepository userRepository, IPassRepository passRepository, IUserMapper userMapper, IPassMapper passMapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passRepository = passRepository ?? throw new ArgumentNullException(nameof(passRepository));
        _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        _passMapper = passMapper ?? throw new ArgumentNullException(nameof(passMapper));
    }
    public async Task<bool> CreatePassAsync(string login, CreatePassRequest request)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        var existingPass = await _passRepository.GetPassengerByPassportAsync(user.Id, request.Passport);
        var existingPass1 = await _passRepository.GetPassengerByEmailAsync(user.Id, request.Email);
        if (existingPass != null)
        {
            throw new ApplicationException("Пассажир с таким паспортом уже существует");
        }
        if (existingPass1 != null)
        {
            throw new ApplicationException("Пассажир с таким email уже существует");
        }

        var age = DateTime.Now.Year - request.Date_birth.Year;
        if (DateTime.Now.Month < request.Date_birth.Month ||
            (DateTime.Now.Month == request.Date_birth.Month && DateTime.Now.Day < request.Date_birth.Day)) age--;

       
            if ((age < 0) || (age > 110))
            {
                throw new ApplicationException("Проверьте дату рождения на корректность");
            }
        
        if (request.Passport?.Length < 10)
        {
            throw new ApplicationException("Серия и номер паспорта 10-значное число");
        }
        if (request.Email?.Length > 50 || request.Email?.Length < 6)
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

        await _passRepository.AddPassenger(_passMapper.Map(request, user.Id));
        return true;
    }
    public async Task<bool> DeletePassengerAsync(string login, DeletePassRequest request)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        await _passRepository.DeletePassenger(user.Id, request.Id_pass);
        return true;
    }

    public async Task<IEnumerable<PassengerDto>> GetPassengerDataAsync(string login)
    {
        var userEntity = await _userRepository.GetUserByLoginAsync(login);
        var passEntity = await _passRepository.GetPassengerAsync(userEntity.Id, userEntity.Email);
        return passEntity.Select(_passMapper.Map);
    }

    public async Task<IEnumerable<PassengerDto>> GetPassengerDataForBookAsync(string login)
    {
        var userEntity = await _userRepository.GetUserByLoginAsync(login);
        var passEntity = await _passRepository.GetAllPassengerAsync(userEntity.Id);
        return passEntity.Select(_passMapper.Map);
    }

    public async Task<bool> IsSelfPassengerAsync(string login, long passengerId)
    {
        return await _passRepository.IsSelfPassengerAsync(login, passengerId);
    }
}
