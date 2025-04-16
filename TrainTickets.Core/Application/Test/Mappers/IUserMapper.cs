using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface IUserMapper
{
    UserDto Map(UserEntity entity, PassengerEntity entity1);
    UserEntity Map(RegisterUserRequest request);

    SessionEntity Map1(UserEntity entity);

    SessionDto Map(SessionEntity entity);

}