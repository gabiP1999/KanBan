using NUnit.Framework;

namespace NUnitTestProject2
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Column c = new Column();
            Assert.Pass();
        }
    }
}