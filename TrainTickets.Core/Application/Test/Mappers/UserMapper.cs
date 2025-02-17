using Org.BouncyCastle.Asn1.Ocsp;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class UserMapper : IUserMapper
{
    public UserDto Map(UserEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new UserDto()
        {
            Id = entity.Id,
            Login = entity.Login,
            Password = entity.Password,
            Email = entity.Email,
            Phone = entity.Phone,
            Surname = entity.Surname,
            Name = entity.Name
        };
    }

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
            Name = request.Name
        };
    }

    public SessionDto Map(SessionEntity request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new SessionDto()
        {
            Guid = request.Guid,
        };
    }

    public SessionEntity Map1(UserEntity request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new SessionEntity()
        {
            Guid = Guid.NewGuid().ToString(),
            UserId = request.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(7)
        };
    }
}