using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Passenger", Schema = "public")]
public class PassengerEntity
{
    [Key]
    public long Id_passenger { get; set; }
    public string? Passport { get; set; }
    public DateTime? Date_birth { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Midname { get; set; }
    public long Id_user {  get; set; }

    public bool Is_self { get; set; }

    [ForeignKey("Id_user")]
    public UserEntity User { get; set; }
}
