using Microsoft.EntityFrameworkCore;
using QueryableProcessor.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBlaze.Processor.Tests.EFCore
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private readonly IEnumerable<Person> _people;

        public DatabaseFixture()
        {
            DbContextOptionsBuilder<TestDbContext> builder = new();

            var personid = 1;
            var addressId = 1;

            Bogus.Faker<Address> addressGen(Person p, int addressId)
                    => new Bogus.Faker<Address>()
                        .RuleFor(x => x.Id, f => addressId++)
                        .RuleFor(x => x.Street, f => f.Address.StreetAddress())
                        .RuleFor(x => x.ZipCode, f => f.Address.ZipCode())
                        .RuleFor(x => x.Person, f => p)
                        .RuleFor(x => x.PersonId, f => p.Id);

            var personGen = new Bogus.Faker<Person>()
                        .RuleFor(x => x.Id, f => personid++)
                        .RuleFor(x => x.Name, f => f.Name.FirstName())
                        .RuleFor(x => x.Surname, f => f.Name.LastName())
                        .RuleFor(x => x.Address, (f, u) => addressGen(u, addressId++));



            _people = Enumerable.Range(0, 200).Select(x => personGen.Generate()).ToList();
        }


        public TestDbContext CreateContext()
        {
            DbContextOptionsBuilder<TestDbContext> builder = new();
            TestDbContext ctx = new(builder.UseInMemoryDatabase("testDb").Options);

            Seed(ctx);

            return ctx;
        }

        private void Seed(TestDbContext ctx)
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();

                    ctx.People.AddRange(_people);

                    ctx.SaveChanges();

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
