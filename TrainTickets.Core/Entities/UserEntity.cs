using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace TrainTickets.UI.Entities;

[Table("User", Schema = "public")]
public class UserEntity
{
    [Key]
    public long Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }

    public string? Midname { get; set; }
}   