using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeoForum.Areas.Identity.Data;
using NeoForum.Models;

namespace NeoForum.Data;

public class NeoForumDbContext : IdentityDbContext<NeoForumUser>
{
    public NeoForumDbContext(DbContextOptions<NeoForumDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Message>()
               .HasOne<NeoForumUser>(a => a.Sender)
               .WithMany(d => d.Messages)
               .HasForeignKey(d => d.UserID);

        builder.Entity<AdminMessage>()
               .HasOne<NeoForumUser>(a => a.AdminSender)
               .WithMany(d => d.AdminMessages)
               .HasForeignKey(d => d.UserID);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Message>? Messages { get; set; }

    public DbSet<AdminMessage>? AdminMessages { get; set; }

    public DbSet<Article>? Articles { get; set; }
}
