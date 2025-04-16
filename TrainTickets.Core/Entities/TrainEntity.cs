using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Train", Schema = "public")]
public class TrainEntity
{
    [Key]
    public int Number_train { get; set; }
    public string? Name { get; set; }
    public int Id_type_train { get; set; }

    public List<VanEntity> Vans { get; set; } = new();

    [ForeignKey("Id_type_train")]
    public TypeTrainEntity Type_train { get; set; }
}
