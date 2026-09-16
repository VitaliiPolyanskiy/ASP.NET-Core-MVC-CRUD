using Microsoft.EntityFrameworkCore;

namespace StudentsMVC.Models
{
    // Контекст даних для підключення до БД через Entity Framework Core
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        // Сучасний підхід до ініціалізації початкових даних (Data Seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Іван", Surname = "Іваненко", Age = 20, GPA = 10.5 },
                new Student { Id = 2, Name = "Сергій", Surname = "Сергієнко", Age = 23, GPA = 11.5 },
                new Student { Id = 3, Name = "Петро", Surname = "Петренко", Age = 25, GPA = 12.0 }
            );
        }
    }
}