﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Domain.User;

public class RegisterUserRequest
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Midname { get; set; }
}
