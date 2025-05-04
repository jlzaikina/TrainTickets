using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Passenger;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface IPassengerHandler
{
    Task<bool> CreatePassAsync(string login, CreatePassRequest request);

    Task<bool> DeletePassengerAsync(string login, DeletePassRequest request);

    Task<IEnumerable<PassengerDto>> GetPassengerDataAsync(string login);
    Task<IEnumerable<PassengerDto>> GetPassengerDataForBookAsync(string login);

    Task<bool> IsSelfPassengerAsync(string login, long passengerId);
}
