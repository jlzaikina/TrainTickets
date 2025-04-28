using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class BookRequest
{
    public int Number_train { get; set; }
    public DateTime DateDeparture { get; set; }
    public int Number_van { get; set; }
    public int Number_seat { get; set; }
    public long Pass_id { get; set; }
    public string type_van { get; set; }
    public string type_seat { get; set; }
    public string Login { get; set; }
}
