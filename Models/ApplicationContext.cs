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
            builder.Entity<PhotoCovers>();
            base.OnModelCreating(builder);
        }
    }
}
