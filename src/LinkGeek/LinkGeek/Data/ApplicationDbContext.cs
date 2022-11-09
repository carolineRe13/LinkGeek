using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinkGeek.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static DbContextOptions<ApplicationDbContext>? Options;
        public DbSet<ChatMessage> ChatMessages { get; set; }
        
        public DbSet<Game> Game { get; set; }
        public DbSet<GameSearchCacheItem> GameSearchCache { get; set; }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
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

            builder.Entity<GameSearchCacheItem>()
                .HasKey(c => new { c.Query, c.Rank });

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.MyFriends)
                .UsingEntity(x =>
                {
                    x.ToTable("Friends");
                });
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.SentFriendRequests)
                .WithMany(u => u.ReceivedFriendRequests)
                .UsingEntity(x =>
                {
                    x.ToTable("FriendRequests");
                });
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.ApplicationUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithMany(u => u.LikedPosts);
            builder.Entity<Post>()
                .Property(p => p.LookingFor)
                .HasConversion(new EnumToStringConverter<PlayerRoles>());

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.ApplicationUser);
        }
    }
}