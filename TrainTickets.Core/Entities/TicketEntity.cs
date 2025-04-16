using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Ticket", Schema = "public")]
public class TicketEntity
{
    [Key]
    public int Id_ticket { get; set; }
    public int Price { get; set; }
    public int Id_seat { get; set; }
    public int Id_book { get; set; }
    public long Id_passenger { get; set; }

    [ForeignKey("Id_seat")]
    public SeatEntity Seat { get; set; }

    [ForeignKey("Id_passenger")]
    public PassengerEntity Passenger { get; set; }

    [ForeignKey("Id_book")]
    public BookEntity Book { get; set; }
}
