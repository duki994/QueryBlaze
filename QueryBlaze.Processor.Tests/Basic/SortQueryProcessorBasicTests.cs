using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace QueryBlaze.Processor.Tests.Basic
{

    public class SortQueryProcessorTests : IDisposable
    {
        private DataFixture _dataFixture;

        public SortQueryProcessorTests()
        {
            _dataFixture = new DataFixture();
        }

        public void Dispose()
        {
            _dataFixture.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Should_Sort_Ascending_When_Has_Sort_Properties_And_Sorting_Indicator_Missing()
        {
            // arrange
            var queryable = _dataFixture.Data.AsQueryable();
            var ordered = _dataFixture.Data.OrderBy(x => x.Data).ToList();

            SortParams p = new();
            p.SortProperties.Add("data");

            var result = _dataFixture.Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_Sort_Descending_When_Has_Sort_Properties_And_Sorting_Indicator_Descending()
        {
            // arrange
            var queryable = _dataFixture.Data.AsQueryable();
            var ordered = _dataFixture.Data.OrderByDescending(x => x.Data).ToList();

            SortParams p = new();
            p.SortProperties.Add("data-");

            var result = _dataFixture.Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }

        [Fact]
        public void Should_MultiSort_Descending_When_Has_Sort_Properties_And_Both_Sorting_Indicators_Descending()
        {
            // arrange
            var queryable = _dataFixture.Data.AsQueryable();
            var ordered = _dataFixture.Data.OrderByDescending(x => x.Data)
                .ThenByDescending(x => x.Description);

            SortParams p = new();
            p.SortProperties.Add("data-");
            p.SortProperties.Add("description-");

            var result = _dataFixture.Processor.ApplySorting(queryable, p);

            result.Should().ContainInOrder(ordered);
        }
    }
}
