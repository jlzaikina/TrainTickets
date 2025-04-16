using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public interface ITicketMapper
{
    BookDto Map(BookEntity entity);
    TicketDto Map(TicketEntity entity);
}
