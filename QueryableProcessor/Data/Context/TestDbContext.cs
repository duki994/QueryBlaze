using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace QueryableProcessor.Data.Context
{
    public class TestDbContext : DbContext
    {
        public DbSet<Person> People => Set<Person>();

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfig());
            modelBuilder.ApplyConfiguration(new AddressConfig());
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;


        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne<Person>()
                .WithOne(x => x.Address!)
                .IsRequired(false);

            builder.HasData(new Address
            {
                Id = 1,
                Street = "Street1",
                ZipCode = "1242"
            },
            new Address
            {
                Id = 2,
                Street = "Street 2",
                ZipCode = "34435"
            },
             new Address
             {
                 Id = 3,
                 Street = "Street 4",
                 ZipCode = "87577"
             });
        }
    }

    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasOne(x => x.Address)
                .WithOne();

            builder.HasData(new Person()
            {
                Id = 1,
                Name = "Tutuban",
                Surname = "Rumunski"
            },
            new Person()
            {
                Id = 2,
                Name = "Tutuban 2",
                Surname = "Madjarski"
            },
            new Person()
            {
                Id = 3,
                Name = "Tutuban 3",
                Surname = "Bugarski"
            });

        }
    }
}
