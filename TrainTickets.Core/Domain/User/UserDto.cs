namespace TrainTickets.UI.Domain.User;

public class UserDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Passport { get; set; }
    public DateTime? Date_birth { get; set; }
    public string? Midname { get; set; }
}