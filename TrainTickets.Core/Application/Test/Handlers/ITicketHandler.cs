using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Handlers;

public interface ITicketHandler
{
    Task<IEnumerable<BookDto>> GetBookAsync(string login);
    Task<TicketDto> GetTicketByIdAsync(int id);
    byte[] GenerateTicketPdf(TicketDto ticket);
    Task<bool> DeleteTicketAsync(int id, string login);
}
