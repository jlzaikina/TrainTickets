using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.Infrastructure.Adapters.Postgres;

public class TicketPostgresRepository: ITicketRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketPostgresRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTicket(TicketEntity entity)
    {
        try
        {
            _dbContext.Tickets.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {

            throw;
        }
    }

    public async Task DeleteTicket(int id, string login)
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id_ticket == id);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);

        if (ticket != null)
        {
            _dbContext.Tickets.Remove(ticket);
            await _dbContext.SaveChangesAsync();
        }

        var tickets = await _dbContext.Tickets
            .Include(t => t.Book)
            .Where(t => t.Id_book == ticket.Id_book && t.Book.Id_user == user.Id).ToListAsync();

      
        if (tickets.Count == 0)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id_book == ticket.Id_book && b.Id_user == user.Id);
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<BookEntity>> GetBookWithDetailsAsync(long id)
    {
        return await _dbContext.Books
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Type_seat)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Van)
                        .ThenInclude(s => s.Type_van)
             .Include(b => b.Tickets)
                .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Van)
                        .ThenInclude(v => v.Train)
            .Include(b => b.Tickets)
                .ThenInclude(t => t.Passenger)
            .Include(b => b.Schedule)
                .ThenInclude(b => b.Route)
                    .ThenInclude(r => r.ArrivalCity)
            .Include(b => b.Schedule)
                .ThenInclude(b => b.Route)
                    .ThenInclude(r => r.DepartureCity)
            .Where(b => b.Id_user == id && b.Schedule.Date_departure >= DateTime.Now.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TicketEntity> GetTicketByIdAsync(int id)
    {
        return await _dbContext.Tickets
           .Include(t => t.Seat)
                   .ThenInclude(s => s.Type_seat)
           .Include(t => t.Seat)
                   .ThenInclude(s => s.Van)
                       .ThenInclude(s => s.Type_van)
            .Include(t => t.Seat)
                   .ThenInclude(s => s.Van)
                       .ThenInclude(v => v.Train)
           .Include(t => t.Passenger)
           .Include(t => t.Book)
                .ThenInclude(b => b.Schedule)
                    .ThenInclude(b => b.Route)
                        .ThenInclude(r => r.ArrivalCity)
           .Include(t => t.Book)
                .ThenInclude(b => b.Schedule)
                    .ThenInclude(b => b.Route)
                        .ThenInclude(r => r.DepartureCity)
           .AsNoTracking()
           .FirstOrDefaultAsync(t => t.Id_ticket == id);
    }
}
