using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static DbContextOptions<ApplicationDbContext>? Options;
        public DbSet<ChatMessage> ChatMessages { get; set; }
        
        public DbSet<Game> Game { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Options = options;
        }
        
        public ApplicationDbContext()
            : base(Options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatMessagesFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatMessagesToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}