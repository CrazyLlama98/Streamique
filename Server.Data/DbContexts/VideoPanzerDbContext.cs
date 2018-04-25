using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Models;

namespace Server.Data.DbContexts
{
    public class VideoPanzerDbContext : IdentityDbContext<User, UserRole, int>
    {
        public VideoPanzerDbContext(DbContextOptions options) 
            : base(options)
        { }

        public DbSet<Lobby> Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Lobby>()
                .HasOne(t => t.Host)
                .WithOne()
                .HasForeignKey(typeof(Lobby), "HostId");
            builder.Entity<Lobby>()
                .Property(t => t.DateCreated)
                .HasDefaultValueSql("GETDATE()");
            builder.Entity<Lobby>()
                .HasMany(t => t.JoinRequests)
                .WithOne()
                .HasForeignKey(t => t.LobbyId);
            builder.Entity<User>()
                .HasMany(t => t.Lobbies)
                .WithOne(t => t.Host)
                .HasForeignKey(t => t.HostId);
        }
    }
}
