using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;

[Table("Session", Schema = "public")]
public class SessionEntity
{
    [Key]
    public string Guid { get; set; }
    public DateTime Expiration_Date { get; set; }
    public long User_Id { get; set; }

    //[ForeignKey("UserId")]
    //public UserEntity User { get; set; }
}
