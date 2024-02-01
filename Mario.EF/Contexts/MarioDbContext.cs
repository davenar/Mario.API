using Mario.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.EF.Contexts
{
    public class MarioDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        public MarioDbContext(DbContextOptions<MarioDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura la tabella per l'entità Courses
            modelBuilder.Entity<Course>().ToTable("Courses");

            // Configura la tabella per l'entità Dish
            modelBuilder.Entity<Dish>()
                .ToTable("Dishes")
                .HasKey(d => d.Id); // Set the primary key

            // Configura la relazione tra Dish e Course
            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Course)              // Un Dish ha una relazione con un Course
                .WithMany(c => c.AvailableDishes)   // Un Course può avere molti AvailableDishes
                .HasForeignKey(d => d.CourseId);    // La chiave esterna su Dish è CourseId

            // Gestisci la colonna Ingredients non mappata
            modelBuilder.Entity<Dish>()
                .Property(d => d.IngredientsAsString)
                .HasColumnName("Ingredients"); // Usa il nome desiderato per la colonna nel database
        }
    }
}
