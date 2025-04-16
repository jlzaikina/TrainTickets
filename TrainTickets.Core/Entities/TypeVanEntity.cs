using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;
[Table("Type_van", Schema = "public")]
public class TypeVanEntity
{
    [Key]
    public int Id_type_van { get; set; }
    public string Name { get; set; }
    public double Route { get; set; }
}
