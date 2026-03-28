using Calendar_App.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calendar_App.Persistence.MsSql
{
    public class AppDbContext : DbContext
    {
        public DbSet<CoachEntity> Coaches { get; set; }
        public DbSet<TrainingEntity> Trainings { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=gymdb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Coaches
            modelBuilder.Entity<CoachEntity>().HasData(
                new CoachEntity { Id = 1, Name = "Gyúrós Gyula", PhoneNumber = 702053211 },
                new CoachEntity { Id = 2, Name = "Gyúrós Gyulus", PhoneNumber = 202053211 },
                 new CoachEntity { Id = 3, Name = "Gyúrós György", PhoneNumber = 302053211 },
                new CoachEntity { Id = 4, Name = "Bicepsz Benő", PhoneNumber = 102053211 },
                new CoachEntity { Id = 5, Name = "Alkar Aladár", PhoneNumber = 103553211 },
                new CoachEntity { Id = 6, Name = "Hasizom Hedvig", PhoneNumber = 108853211 },
                new CoachEntity { Id = 7, Name = "Tricepsz Tamás", PhoneNumber = 107753211 },
                new CoachEntity { Id = 8, Name = "Vádli Vendel", PhoneNumber = 702099911 }
            );

            // Seed Trainings
            modelBuilder.Entity<TrainingEntity>().HasData(
               new TrainingEntity { Id = 1, Location = "kis terem", Level = "haladó", Type = "pilátesz", Beginning = new DateTime(2025, 5, 10, 16, 40, 0), End = new DateTime(2025, 5, 10, 17, 40, 0), SignUps = 4, CoachEntityId = 1 },
               new TrainingEntity { Id = 2, Location = "közepes terem", Level = "kezdő", Type = "pilátesz", Beginning = new DateTime(2025, 5, 15, 17, 40, 0), End = new DateTime(2025, 5, 15, 18, 40, 0), SignUps = 5, CoachEntityId = 2 },
               new TrainingEntity { Id = 3, Location = "kis terem", Level = "haladó", Type = "pilátesz", Beginning = new DateTime(2025, 5, 14, 17, 40, 0), End = new DateTime(2025, 5, 14, 18, 40, 0), SignUps = 3, CoachEntityId = 3 },
               new TrainingEntity { Id = 4, Location = "közepes terem", Level = "középhaladó", Type = "pilátesz", Beginning = new DateTime(2025, 5, 13, 17, 40, 0), End = new DateTime(2025, 5, 13, 18, 40, 0), SignUps = 5, CoachEntityId = 4 },
               new TrainingEntity { Id = 5, Location = "nagy terem", Level = "haladó", Type = "pilátesz", Beginning = new DateTime(2025, 5, 12, 17, 40, 0), End = new DateTime(2025, 5, 12, 18, 40, 0), SignUps = 10, CoachEntityId = 5 },
               new TrainingEntity { Id = 6, Location = "nagy terem", Level = "kezdő", Type = "pilátesz", Beginning = new DateTime(2025, 5, 11, 17, 40, 0), End = new DateTime(2025, 5, 11, 18, 40, 0), SignUps = 10, CoachEntityId = 6 },
               new TrainingEntity { Id = 7, Location = "nagy terem", Level = "középhaladó", Type = "pilátesz", Beginning = new DateTime(2025, 5, 20, 17, 40, 0), End = new DateTime(2025, 5, 20, 18, 40, 0), SignUps = 8, CoachEntityId = 7 },
                new TrainingEntity { Id = 8, Location = "nagy terem", Level = "kezdő", Type = "pilátesz", Beginning = new DateTime(2025, 5, 21, 17, 40, 0), End = new DateTime(2025, 5, 21, 18, 40, 0), SignUps = 9, CoachEntityId = 8 }
           );
        }





    }
}
