using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class CreateVanRequest
{
    public int VanNumber { get; set; }
    public int SchemaId { get; set; }
}
