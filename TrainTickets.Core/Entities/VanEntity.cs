using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;
[Table("Van", Schema = "public")]
public class VanEntity
{
    [Key]
    public int Id_van { get; set; }
    public int Count_seats { get; set; }
    public int Number_van { get; set; }
    public int Number_train { get; set; }
    public int Id_type_van { get; set; }
    public int Id_schema { get; set; }

    [Column(TypeName = "jsonb")]
    public string Copy_schema { get; set; }
    public List<SeatEntity> Seats { get; set; } = new();

    [ForeignKey("Number_train")]
    public TrainEntity Train { get; set; }

    [ForeignKey("Id_type_van")]
    public TypeVanEntity Type_van { get; set; }

    [ForeignKey("Id_schema")]
    public SchemaEntity Schema { get; set; }
}
