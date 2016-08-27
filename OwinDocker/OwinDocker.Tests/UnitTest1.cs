using FsCheck.Xunit;
using Xunit;

namespace OwinDocker.Tests
{
    public class UnitTest1
    {
     
        [Property(Arbitrary = new[] { typeof(SomeArbitrary) })]
        public void TestMethod1(SomeModel some, int value)
        {
            Assert.NotNull("");
            Assert.NotEqual(0,1);
        }
    }
}
