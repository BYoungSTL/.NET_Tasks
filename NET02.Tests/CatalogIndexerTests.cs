using System;
using System.Collections.Generic;
using NET02.Entities;
using Xunit;
using Assert = NUnit.Framework.Assert;


namespace NET02.Tests
{
    public class CatalogIndexerTests
    {
        [Fact]
        public void Indexer_888_8_88_888888_8_DoomedCityReturned()
        {
            Catalog catalog = Program.InitCatalog();

            Book expected = new Book
            (
                "888-8-88-888888-8",
                "Doomed City",
                new List<Author> {new("Boris", "Strugatckiy"), new("Arkadiy", "Strugatckiy")},
                new DateTime(1972, 04, 27)
            );
            
            Assert.AreEqual(catalog["888-8-88-888888-8"], expected);
        }
    }
}