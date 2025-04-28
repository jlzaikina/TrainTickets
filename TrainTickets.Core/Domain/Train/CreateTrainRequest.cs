using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class CreateTrainRequest
{
    public int TrainNumber {  get; set; }
    public string TrainType { get; set; }
    public string? TrainName { get; set; }
    public List<CreateVanRequest> Vans { get; set; }
}
