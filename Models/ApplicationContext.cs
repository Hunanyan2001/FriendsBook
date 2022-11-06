using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IProject.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                   : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<PhotoCovers> PhotoCovers { get; set; }

        public DbSet<UserFriendShip> Friends { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<FileModel>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10000000)
                    .HasMaxLength(10000000)
                    .HasMaxLength(10000000);
            });
            builder.Entity<UserFriendShip>(b =>
            {
                b.HasKey(x => new { x.UserId, x.UserFriendId });

                b.HasOne(x => x.User)
                    .WithMany(x => x.Friends)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.UserFriend)
                    .WithMany(x => x.FriendsOf)
                    .HasForeignKey(x => x.UserFriendId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            base.OnModelCreating(builder);
        }
    }
}
