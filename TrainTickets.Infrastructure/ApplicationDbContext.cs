﻿using Microsoft.EntityFrameworkCore;
using TrainTickets.UI.Entities;

namespace TrainTickets.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<SessionEntity> Sessions { get; set; } = null!;

    public DbSet<ScheduleEntity> Schedules { get; set; } = null!;
    public DbSet<PassengerEntity> Passengers { get; set; } = null!;

    public DbSet<BookEntity> Books { get; set; } = null!;
    public DbSet<TicketEntity> Tickets { get; set; } = null!;
    public DbSet<TrainEntity> Trains { get; set; } = null!;

    public DbSet<VanEntity> Vans { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}