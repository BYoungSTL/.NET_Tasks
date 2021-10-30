using NET02.Entities;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace NET02.Tests
{
    public class ISBNCodeTests
    {
        [Fact]
        public void ISBNCode_111_1_11_111111_1__1111111111111returned()
        {
            //arrange
            string expected = "1111111111111";
            
            //act
            string isbnCode = Book.ISBNCodeСoercion("111-1-11-111111-1");

            //assert
            Assert.AreEqual(expected, isbnCode);
        }
    }
}