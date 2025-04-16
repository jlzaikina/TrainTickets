using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Ticket;

public class TicketDto
{
    public int Id_ticket { get; set; }
    public int Train_number { get; set; }
    public string? Train_name { get; set; }
    public int Train_van { get; set; }
    public string Van_name { get; set; }
    public int Train_seat { get; set; }
    public string Seat_name { get; set; }
    public string Departure_time { get; set; }
    public string Arrival_time { get; set; }
    public string Departure_city_name { get; set; } // Город отправления
    public string Arrival_city_name { get; set; } // Город прибытия

    public string Passenger_name { get; set; }

    public string Passenger_date_birth { get; set; }

    public string? Passenger_passport { get; set; }
    public string PdfUrl { get; set; }
}
