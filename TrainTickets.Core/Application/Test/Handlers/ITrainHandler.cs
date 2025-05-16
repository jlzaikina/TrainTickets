using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface ITrainHandler
{
    Task<TrainDto> GetInfoTrainInScheduleAsync(InfoTrainRequest request);
    Task<VanDto> GetShemaVanAsync(InfoVanRequest request);
    Task<double> GetPriceAsync(CheckRequest request);
    Task<TicketDto> CreateBookAsync(BookRequest request);
    Task<IEnumerable<int>> GetVanNumberAsync();

    Task<IEnumerable<SchemaDto>> GetAllSchemaAsync();
    Task<SchemaDto> GetSchemaAsync(int id);
    Task<bool> SaveSchemaAsync(SaveSchemaRequest request);

    Task<bool> UpdateSchemaAsync(int id, SaveSchemaRequest request);

    Task<bool> DeleteSchemaAsync(int id);

    Task<bool> CreateTrainAsync(CreateTrainRequest request);

    Task<bool> DeleteTrainAsync(int number);

    Task<IEnumerable<TrainDetailsDto>> GetTrainsAsync();

    Task<bool> UpdateTrainAsync(TrainDetailsDto request);

    Task<IEnumerable<string>> GetTypeTrainsAsync();

    Task<bool> CheckUpdateTrainAsync(int numberTrain);

    Task<bool> CheckUpdateSchemaAsync(int schemaId);
}
