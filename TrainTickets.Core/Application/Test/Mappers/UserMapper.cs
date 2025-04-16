using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Tls;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class UserMapper : IUserMapper
{
    public UserEntity Map(RegisterUserRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new UserEntity()
        {
            Login = request.Login,
            Email = request.Email,
            Password = request.Password,
            Phone = request.Phone,
            Surname = request.Surname,
            Name = request.Name,
            Midname = request.Midname  
        };
    }

    public SessionDto Map(SessionEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new SessionDto()
        {
            Guid = entity.Guid,
        };
    }

    public SessionEntity Map1(UserEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new SessionEntity()
        {
            Guid = Guid.NewGuid().ToString(),
            User_Id = entity.Id,
            Expiration_Date = DateTime.UtcNow.AddDays(7)
        };
    }

    public UserDto Map(UserEntity entity, PassengerEntity entity1)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new UserDto()
        {
            Name = entity.Name,
            Surname = entity.Surname,
            Midname= entity.Midname,
            Email = entity.Email,
            Phone = entity.Phone,
            Passport = entity1?.Passport,
            Date_birth = entity1?.Date_birth
        };
    }
}