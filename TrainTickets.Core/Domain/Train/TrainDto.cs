using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class TrainDto
{
    public int Number_train { get; set; }
    public string DateDeparture { get; set; }
    public string DateArrival { get; set; }
    public string Route { get; set; }

    public TimeSpan TravelDuration { get; set; }

    public string TypeTrain {  get; set; }

    public Dictionary<string, List<int>> VanByType { get; set; }
}
