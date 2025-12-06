using Microsoft.EntityFrameworkCore;

public class EVSDbContext : DbContext
{
    public EVSDbContext(DbContextOptions<EVSDbContext> options) : base(options) { }

    public DbSet<Administrator> Administrators { get; set; }
}

public class Administrator
{
    public int AdminID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
}
