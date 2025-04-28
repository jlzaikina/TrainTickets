using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

public interface IPassRepository
{
    Task<PassengerEntity> GetPassengerByEmailAsync(long id, string email);
    Task<PassengerEntity> GetPassengerByPassportAsync(long id, string passport);
    Task AddPassenger(PassengerEntity entity);
    Task DeletePassenger(long id, long id_pass);
    Task<IEnumerable<PassengerEntity>> GetPassengerAsync(long id, string email);

    Task<bool> IsEmailUniqueForPassAsync(long id, string email);
    Task<bool> IsPassportUniqueGlobalAsync(string passport, long? excludePassengerId = null);

    Task<bool> IsPassportUniqueForUserAsync(string passportNumber, long userId, long? excludeUserId = null);

    Task<IEnumerable<PassengerEntity>> GetAllPassengerAsync(long id);
}
