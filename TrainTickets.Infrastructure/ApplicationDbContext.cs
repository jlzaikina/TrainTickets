using Microsoft.EntityFrameworkCore;
using TrainTickets.UI.Entities;

namespace TrainTickets.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<SessionEntity> Sessions { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}