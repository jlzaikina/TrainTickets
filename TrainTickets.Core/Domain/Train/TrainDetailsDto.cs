using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class TrainDetailsDto
{
    public int TrainNumber {  get; set; }
    public string TrainType { get; set; }
    public string TrainName { get; set; }
    public List<VanDetailsDto> Vans { get; set; }
}
