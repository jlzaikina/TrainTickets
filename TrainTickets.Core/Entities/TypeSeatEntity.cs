using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;
[Table("Type_seat", Schema = "public")]
public class TypeSeatEntity
{
    [Key]
    public int Id_type_seat { get; set; }
    public string Name { get; set; }
    public double Route { get; set; }
}
