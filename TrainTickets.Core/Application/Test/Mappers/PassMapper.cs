using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Passenger;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class PassMapper: IPassMapper
{
    public PassengerEntity Map(UserEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new PassengerEntity()
        {
            Surname = entity.Surname,
            Name = entity.Name,
            Email = entity.Email,
            Id_user = entity.Id,
            Midname = entity.Midname,
            Is_self = true
        };
    }
    public PassengerEntity Map(CreatePassRequest request, long id)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new PassengerEntity()
        {
            Passport = request.Passport,
            Date_birth = request.Date_birth,
            Surname = request.Surname,
            Name = request.Name,
            Email = request.Email,
            Midname = request.Midname,
            Id_user = id
        };
    }
    public PassengerDto Map(PassengerEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new PassengerDto()
        {
            Id_pass = entity.Id_passenger,
            Passport = entity.Passport,
            Date_birth = entity.Date_birth,
            Surname = entity.Surname,
            Name = entity.Name,
            Email = entity.Email,
            Midname = entity.Midname,
        };
    }
}
