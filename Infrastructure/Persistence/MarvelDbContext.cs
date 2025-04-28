using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class MarvelDbContext : DbContext
    {
        public MarvelDbContext(DbContextOptions<MarvelDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteComic> FavoriteComics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar las relaciones si quieres
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteComics)
                .WithOne(fc => fc.User)
                .HasForeignKey(fc => fc.UserId);
        }
    }
}
