using NUnit.Framework;
using DollarAddresses;
using System.Collections.Generic;

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
            var Address = new Object.Address { ADDRESS_NUMBER = 3, STREETNAME = "A", SUFFIX = "B" };
            var streetNameValue = Program.getWordValue(Address.STREETNAME);
            var streetSuffixValue = Program.getWordValue(Address.SUFFIX);
            var result = Program.IsItDollarAddress(streetNameValue, streetSuffixValue, Address.ADDRESS_NUMBER);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsItDollarAddress_ItIsNotDollarAddress_ReturnsTrue()
        {
            var Address = new Object.Address { ADDRESS_NUMBER = 127, STREETNAME = "B", SUFFIX = "C" };
            var streetNameValue = Program.getWordValue(Address.STREETNAME);
            var streetSuffixValue = Program.getWordValue(Address.SUFFIX);
            var result = Program.IsItDollarAddress(streetNameValue, streetSuffixValue, Address.ADDRESS_NUMBER);
            Assert.IsFalse(result);
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
            return Program.getWordValue(input);
        }
    }
}