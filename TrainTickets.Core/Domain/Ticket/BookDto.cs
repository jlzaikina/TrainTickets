using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Ticket;

public class BookDto
{
    public int Id_book { get; set; }
    public List<TicketDto> Tickets { get; set; }
}
