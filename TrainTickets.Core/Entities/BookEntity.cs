using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Book", Schema = "public")]
public class BookEntity
{
    [Key]
    public int Id_book { get; set; }
    public DateTime Date_create { get; set; }
    public int Id_schedule { get; set; }
    public long Id_user { get; set; }

    public List<TicketEntity> Tickets { get; set; } = new();

    [ForeignKey("Id_schedule")]
    public ScheduleEntity Schedule { get; set; }

    [ForeignKey("Id_user")]
    public UserEntity User { get; set; }

}
