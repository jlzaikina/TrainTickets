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

    Task<IEnumerable<int>> GetVanNumberAsync();
    Task<IEnumerable<SchemaEntity>> GetAllSchemaAsync();
    Task<IEnumerable<string>> GetSchemaNameAsync();
    Task<string> GetSchemaNameByIdAsync(int id);
    Task AddSchema(SchemaEntity entity);

    Task<SchemaEntity> GetSchemaByIdAsync(int id);

    Task<TrainEntity> GetTrainByNumberAsync(int number);
    Task DeleteTrain(TrainEntity entity);

    Task UpdateSchema(SchemaEntity entity);

    Task DeleteSchema(SchemaEntity entity);

    Task<int> GetTypeTrainIdAsync(string type);
    Task<int> GetTypeVanIdAsync(string type);
    Task<int> GetTypeSeatIdAsync(string type);

    Task AddTrain(TrainEntity entity);

    Task<IEnumerable<TrainEntity>> GetAllTrainsAsync();
    Task DeleteVans(IEnumerable<VanEntity> entity);

    Task UpdateTrain(TrainEntity entity);

    Task<IEnumerable<string>> GetTypeTrainsAsync();

    Task<bool> CheckUpdateTrainAsync(int numberTrain);

    Task<bool> CheckUpdateSchemaAsync(int schemaId);
}
