using QueryBlaze.Processor.Implementation;
using System;
using System.Collections.Generic;

namespace QueryBlaze.Processor.Tests.Basic
{
    public class DataFixture : IDisposable
    {
        public IEnumerable<DataContainer> Data { get; set; } = new DataContainer[]
            {
                new()
                {
                    Data = "123",
                    Description = "A number 123",
                },
                new()
                {
                    Data = "456",
                    Description = "Breaking Bad 456",
                },
                new()
                {
                    Data = "789",
                    Description = "Crockodile",
                },
                new()
                {
                    Data = "101112",
                    Description = "Drawing",
                }
            };

        public SortQueryProcessor Processor { get; } = new SortQueryProcessor(new DefaultSortProcessorOptionsProvider(), new DefaultCustomPropertyMapper());

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
        }
    }
}
