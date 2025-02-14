namespace TrainTickets.UI.Domain.User;

public class UserDto
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
}