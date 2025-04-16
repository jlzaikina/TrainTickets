using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Passenger;

public class PassengerDto
{
    public long Id_pass { get; set; }
    public string Passport { get; set; }
    public DateTime? Date_birth { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Midname { get; set; }
}
