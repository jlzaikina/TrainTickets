using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("City", Schema = "public")]
public class CityEntity
{
    [Key]
    public int Code_city { get; set; }
    public string Name { get; set; }
}
