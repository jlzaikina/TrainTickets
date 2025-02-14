using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface IUserMapper
{
    UserDto Map(UserEntity entity);

    UserEntity Map(RegisterUserRequest request);

}