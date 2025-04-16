using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Schedules;

public class ScheduleDto
{
    public int NumberTrain { get; set; }
    public string DepartureTime { get; set; }
    public string ArrivalTime { get; set; }  
    public string DepartureCityName { get; set; } // Город отправления
    public string ArrivalCityName { get; set; } // Город прибытия
}
