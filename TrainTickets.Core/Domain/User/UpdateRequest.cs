using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.User;

public class UpdateRequest
{
    public string Name { get; set; } // Имя
    public string Surname { get; set; } // Фамилия
    public string Phone { get; set; } // Телефон
    public string Email { get; set; } // Email
    public string? Passport { get; set; }
    public DateTime? Date_birth { get; set; }
    public string? Midname { get; set; }
}
