using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.Train;

public class VanDto
{
    public string CarriageSchemaJson { get; set; } // Схема вагона в JSON
    public List<int> OccupiedSeatNumbers { get; set; } // Список занятых мест
}
