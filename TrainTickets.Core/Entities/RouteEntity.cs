using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Route", Schema = "public")]
public class RouteEntity
{
    [Key]
    public int Id_route { get; set; }
    public int City_departure { get; set; }
    public int City_arrival { get; set; }
    public int Distance { get; set; }

    [ForeignKey("City_departure")]
    public CityEntity DepartureCity { get; set; }

    [ForeignKey("City_arrival")]
    public CityEntity ArrivalCity { get; set; }
}
