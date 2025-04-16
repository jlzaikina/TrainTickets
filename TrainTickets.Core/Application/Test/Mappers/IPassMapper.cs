using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Passenger;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface IPassMapper
{
    PassengerEntity Map(UserEntity entity);
    PassengerEntity Map(CreatePassRequest request, long id);
    PassengerDto Map(PassengerEntity entity);
}
