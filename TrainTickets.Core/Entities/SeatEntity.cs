using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;
[Table("Seat", Schema = "public")]

public class SeatEntity
{
    [Key]
    public int Id_seat { get; set; }
    public int Number_seat { get; set; }
    public int Id_van { get; set; }
    public int Id_type_seat { get; set; }

    [ForeignKey("Id_van")]
    public VanEntity Van { get; set; }

    [ForeignKey("Id_type_seat")]
    public TypeSeatEntity Type_seat { get; set; }
}
