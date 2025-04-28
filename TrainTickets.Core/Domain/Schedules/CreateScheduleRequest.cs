using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Schedules;

public class CreateScheduleRequest
{
    public int NumberTrainNew { get; set; }
    public DateTime DepartureTimeNew { get; set; }
    public DateTime ArrivalTimeNew { get; set; }
    public string DepartureCityNameNew { get; set; } // Город отправления
    public string ArrivalCityNameNew { get; set; } // Город прибытия
}
