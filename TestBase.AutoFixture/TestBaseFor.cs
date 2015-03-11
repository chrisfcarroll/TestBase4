using System;
using NUnit.Framework;

namespace TestBase
{
    public class TestBaseFor<T>
    {
        protected internal T UnitUnderTest;

        [SetUp]
        protected virtual void CreateUnitUnderTest()
        {
            UnitUnderTest = Activator.CreateInstance<T>();
        }

    }
}
