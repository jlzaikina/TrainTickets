using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

public interface ITrainRepository
{
    Task<ScheduleEntity> GetInfoTrainInScheduleAsync(InfoTrainRequest request);
    Task<VanEntity> GetShemaVanAsync(InfoVanRequest request);

    Task<List<int>> GetOccupiedSeatAsync(int id);

    Task<int> GetActiveBookingsCountAsync(long id);

    Task<SeatEntity> GetByNumberAsync(int seatNumber, int vanNumber, int trainId);

    Task<bool> IsSeatAvailableAsync(int idSeat, int idSchedule);
    Task<bool> HasTicketForScheduleAsync(long id, int idSchedule);
    Task<BookEntity?> GetActiveBookingForScheduleAsync(long id, int idSchedule);

    Task AddBook(BookEntity entity);
}
