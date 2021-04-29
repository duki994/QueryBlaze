using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QueryBlaze.Processor.Abstractions;
using QueryBlaze.Processor.Implementation;
using System.Linq;
using Xunit;

namespace QueryBlaze.Processor.Tests.EFCore
{
    public class SortQueryProcessorEFTests : IClassFixture<DatabaseFixture>
    {
        public DatabaseFixture Fixture { get; }
        public SortQueryProcessorEFTests(DatabaseFixture fixture) => Fixture = fixture;

        public static ISortProcessorOptionsProvider OptionsProvider { get; } = new DefaultSortProcessorOptionsProvider();
        public static SortQueryProcessor Processor { get; } = new SortQueryProcessor(OptionsProvider);
        public static LambdaExpressionFactory Utils { get; } = new LambdaExpressionFactory(OptionsProvider);

        [Fact]
        public void Should_Sort_Ascending_When_Has_Sort_Property_And_Sorting_Indicator_Missing()
        {
            using var context = Fixture.CreateContext();

            var queryable = context.People.AsQueryable();
            var ordered = queryable.OrderBy(x => x.Id).ToList();

            SortParams p = new();
            p.SortProperties.Add("id");

            var result = Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_Sort_Ascending_When_Has_Nested_Sort_Property_And_Sorting_Indicator_Missing()
        {
            using var context = Fixture.CreateContext();

            var queryable = context.People.AsQueryable();
            var ordered = queryable.OrderBy(x => x.Address.Street).ToList();

            SortParams p = new();
            p.SortProperties.Add("address.Street");

            var result = Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_Sort_Ascending_When_Has_Nested_Sort_Properties_And_Sorting_Indicator_Missing()
        {
            using var context = Fixture.CreateContext();

            var queryable = context.People.AsQueryable();
            var ordered = queryable.OrderBy(x => x.Id)
                .ThenBy(x => x.Address.Street).ToList();

            SortParams p = new();
            p.SortProperties.Add("id");
            p.SortProperties.Add("address.Street");

            var result = Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_Sort_When_Has_Nested_Sort_Properties_And_Different_Sorting_Indicators_Per_Property()
        {
            using var context = Fixture.CreateContext();

            var queryable = context.People.Include(x => x.Address).AsQueryable();
            var ordered = queryable.OrderBy(x => x.Name)
                .ThenByDescending(x => x.Address.Street).ToList();

            SortParams p = new();
            p.SortProperties.Add("name+");
            p.SortProperties.Add("address.Street-");

            var result = Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_Throw_BadMemberAccessException_When_PropertyName_Incorrect()
        {
            using var context = Fixture.CreateContext();

            var queryable = context.People.Include(x => x.Address).AsQueryable();
            var ordered = queryable.OrderBy(x => x.Name)
                .ThenByDescending(x => x.Address.Street).ToList();

            SortParams p = new();
            p.SortProperties.Add("name+");
            p.SortProperties.Add("address.Streetsss-");


            Assert.Throws<BadMemberAccessException>(() => Processor.ApplySorting(queryable, p));
        }
    }
}
