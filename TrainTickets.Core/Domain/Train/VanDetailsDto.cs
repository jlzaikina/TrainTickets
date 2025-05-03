using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class VanDetailsDto
{
    public int VanNumber { get; set; }
    public int SchemaId { get; set; }
    public string Schema { get; set; }
}
