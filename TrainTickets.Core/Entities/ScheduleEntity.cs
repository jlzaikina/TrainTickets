using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Schedule", Schema = "public")]
public class ScheduleEntity
{
    [Key]
    public int Id_schedule { get; set; }
    public DateTime Date_departure { get; set; }
    public DateTime Date_arrival { get; set; }
    public int Id_route { get; set; }
    public int Number_train { get; set; }

    [ForeignKey("Id_route")]
    public RouteEntity Route { get; set; }

    [ForeignKey("Number_train")]
    public TrainEntity Train { get; set; }
}
