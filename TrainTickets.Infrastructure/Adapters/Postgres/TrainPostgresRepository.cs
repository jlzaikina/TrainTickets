using Microsoft.EntityFrameworkCore;
using Npgsql;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

public class TrainPostgresRepository : ITrainRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TrainPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddBook(BookEntity entity)
    {
        try
        {
            _dbContext.Books.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task AddSchema(SchemaEntity entity)
    {
        try
        {
            _dbContext.Schemas.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task AddTrain(TrainEntity entity)
    {
        try
        {
            _dbContext.Trains.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task DeleteSchema(SchemaEntity entity)
    {
        try
        {
            _dbContext.Schemas.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task<BookEntity?> GetActiveBookingForScheduleAsync(long id, int idSchedule)
    {
        return await _dbContext.Books
         .Include(b => b.Tickets)
         .FirstOrDefaultAsync(b => b.Id_user == id &&
                                 b.Id_schedule == idSchedule);
    }
    public async Task<int> GetActiveBookingsCountAsync(long id)
    {
        return await _dbContext.Books
             .CountAsync(b => b.Id_user == id);
    }

    public async Task<IEnumerable<SchemaEntity>> GetAllSchemaAsync()
    {
        return await _dbContext.Schemas.ToListAsync();
    }

    public async Task<SeatEntity> GetByNumberAsync(int seatNumber, int vanNumber, int trainId)
    {
        return await _dbContext.Seats
            .Include(s => s.Van)
            .Where(s => s.Van.Number_train == trainId &&
                       s.Van.Number_van == vanNumber &&
                       s.Number_seat == seatNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<ScheduleEntity> GetInfoTrainInScheduleAsync(InfoTrainRequest request)
    {
       return await _dbContext.Schedules
            .Include(s => s.Route)
                .ThenInclude(r => r.DepartureCity)
            .Include(s => s.Route)
                .ThenInclude(r => r.ArrivalCity)
            .Include(s => s.Train)
                .ThenInclude(t => t.Type_train)
            .Include(s => s.Train)
                .ThenInclude(t => t.Vans)
                    .ThenInclude(v => v.Type_van)
            .Include(s => s.Train)
                .ThenInclude(t => t.Vans)
                    .ThenInclude(v => v.Seats)
                        .ThenInclude(s => s.Type_seat)
            .FirstOrDefaultAsync(t => t.Number_train == request.Number_train && t.Date_departure.Date == request.DateDeparture.Date);


    }

    public async Task<List<int>> GetOccupiedSeatAsync(int id)
    {
        return await _dbContext.Tickets
            .Include(t => t.Seat)
                .ThenInclude(s => s.Van)
            .Where(t => t.Seat.Van.Id_van == id)
            .Select(t => t.Seat.Number_seat)
            .ToListAsync();
    }

    public async Task<SchemaEntity> GetSchemaByIdAsync(int id)
    {
        return await _dbContext.Schemas
            .FirstOrDefaultAsync(v => v.Id_schema == id);
    }

    public async Task<IEnumerable<string>> GetSchemaNameAsync()
    {
        return await _dbContext.Schemas
            .FromSqlRaw("SELECT \"Schema\"->>'schemaName' AS \"Schema\" FROM \"Schema\"")
            .Select(s => s.Schema)
            .ToListAsync();
        
    }

    public async Task<string> GetSchemaNameByIdAsync(int id)
    {
        return await _dbContext.Schemas
            .FromSqlRaw("SELECT \"Schema\"->>'schemaName' AS \"Schema\", \"Id_schema\" FROM \"Schema\" WHERE \"Id_schema\" = {0}", id)
            .Select(s => s.Schema)
            .FirstOrDefaultAsync();
    }

    public async Task<VanEntity> GetShemaVanAsync(InfoVanRequest request)
    {
        return await _dbContext.Vans
            .Include(v => v.Schema)
            .FirstOrDefaultAsync(v => v.Number_train == request.Number_train && v.Number_van == request.Number_van);

    }

    public async Task<int> GetTypeTrainIdAsync(string type)
    {
        return await _dbContext.TypeTrains
            .Where(t => t.Name == type)
            .Select(t => t.Id_type_train)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetTypeSeatIdAsync(string type)
    {
        return await _dbContext.TypeSeats
            .Where(t => t.Name == type)
            .Select(t => t.Id_type_seat)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetTypeVanIdAsync(string type)
    {
        return await _dbContext.TypeVans
            .Where(t => t.Name == type)
            .Select(t => t.Id_type_van)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<int>> GetVanNumberAsync()
    {
        return await _dbContext.Trains
           .Select(t => t.Number_train)
           .ToListAsync();
    }

    public async Task<bool> HasTicketForScheduleAsync(long id, int idSchedule)
    {
        return await _dbContext.Tickets
            .Include(t => t.Book)
            .AnyAsync(t => t.Id_passenger == id &&
                          t.Book.Id_schedule == idSchedule);
    }

    public async Task<bool> IsSeatAvailableAsync(int idSeat, int idSchedule)
    {
        return !await _dbContext.Tickets
            .Include(t => t.Book)
            .AnyAsync(t => t.Id_seat == idSeat &&
                          t.Book.Id_schedule == idSchedule);
    }

    public async Task UpdateSchema(SchemaEntity entity)
    {
        try
        {
            _dbContext.Schemas.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            throw;
        }
    }

    public async Task<TrainEntity> GetTrainByNumberAsync(int number)
    {
        return await _dbContext.Trains
            .Include(t=>t.Vans)
                .ThenInclude(v=>v.Seats)
            .FirstOrDefaultAsync(t => t.Number_train == number);
    }

    public async Task DeleteTrain(TrainEntity entity)
    {
        try
        {
            _dbContext.Trains.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task<IEnumerable<TrainEntity>> GetAllTrainsAsync()
    {
        return await _dbContext.Trains
            .Include(t=>t.Vans)
              .ThenInclude(v=>v.Schema)
            .Include(t => t.Type_train)
            .ToListAsync();
    }

    public async Task DeleteVans(IEnumerable<VanEntity> entity)
    {
        try
        {
            _dbContext.Vans.RemoveRange(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task UpdateTrain(TrainEntity entity)
    {
        try
        {
            _dbContext.Trains.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetTypeTrainsAsync()
    {

        return await _dbContext.TypeTrains
            .Select(t => t.Name)
            .ToListAsync();
    }

    public async Task<bool> CheckUpdateTrainAsync(int numberTrain)
    {
        return await _dbContext.Schedules
            .Include(t => t.Train)
            .AnyAsync(t => t.Train.Number_train== numberTrain);
    }
}
