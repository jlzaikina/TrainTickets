using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

public interface ITicketRepository
{
    Task<IEnumerable<BookEntity>> GetBookWithDetailsAsync(long id);
    Task<TicketEntity> GetTicketByIdAsync(int id);

    Task DeleteTicket(int id, string login);

    Task AddTicket(TicketEntity entity);
}
