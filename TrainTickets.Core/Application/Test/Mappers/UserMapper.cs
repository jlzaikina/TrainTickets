using TrainTickets.UI.Domain;
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
            Email = entity.Email
        };
    }
}