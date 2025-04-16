using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class InfoTrainRequest
{
    public int Number_train { get; set; }
    public DateTime DateDeparture { get; set; }
}
