using System;
using FsCheck;

namespace OwinDocker.Tests
{
    public static class SomeArbitrary
    {
        public static  Arbitrary<SomeModel> Inventories()
        {
            var genSome = from name in Arb.Generate<Guid>()
                from value in Arb.Generate<int>()
                select new SomeModel(name.ToString(), value);

            return genSome.ToArbitrary();
        }
    }
}