using NUnit.Framework;
using DollarAddresses;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsItDollarAddress_ItIsDollarAddress_ReturnsTrue()
        {
            //Arrange
            var Address = new DollarAddresses.Object.Address { ADDRESS_NUMBER = 3, STREETNAME = "A", SUFFIX = "B" };
            var streetNameValue = DollarAddresses.Program.getWordValue(Address.STREETNAME);
            var streetSuffixValue = DollarAddresses.Program.getWordValue(Address.SUFFIX);
            //Act
            var result = DollarAddresses.Program.IsItDollarAddress(streetNameValue, streetSuffixValue, Address.ADDRESS_NUMBER);
            //Assert
            Assert.IsTrue(result);
        }

        [TestCase("a", ExpectedResult = 1)]
        [TestCase("a?a", ExpectedResult = 2)]
        [TestCase("C", ExpectedResult = 3)]
        [TestCase("  ", ExpectedResult = 0)]
        [TestCase("a B c", ExpectedResult = 6)]
        [TestCase("A,+b-- &ca", ExpectedResult = 7)]
        [TestCase("      z", ExpectedResult = 26)]
        [TestCase("1ac ", ExpectedResult = 4)]
        [TestCase("145?.&%99.0  6", ExpectedResult = 0)]
        public int TestWordValue(string input)
        {
            return DollarAddresses.Program.getWordValue(input);
        }
    }
}