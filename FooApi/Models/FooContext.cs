using Microsoft.EntityFrameworkCore;

namespace FooApi.Models
{
    public class FooContext : DbContext
    {
        public FooContext(DbContextOptions<FooContext> options): base(options)
        {
        }

        public DbSet<Foo> Foo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Foo>()
                .Property(f => f.Id)
                .ForSqlServerUseSequenceHiLo();

            modelBuilder.Entity<Foo>()
                .Property(f => f.Bar)
                .IsRequired();
        }
    }
}