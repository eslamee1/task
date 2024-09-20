
using Microsoft.EntityFrameworkCore;
using Survey_Task.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define DbSet properties for each model
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<ScannedQRCode> ScannedQRCodes { get; set; }


}
