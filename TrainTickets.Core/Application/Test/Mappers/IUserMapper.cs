using TrainTickets.UI.Domain;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface IUserMapper
{
    UserDto Map(UserEntity entity);
}