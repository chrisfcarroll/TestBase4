using NUnit.Framework;

namespace TestBase
{
    /// <summary>
    /// A Base Class for TestFixtures / TestClasses which will auto-construct a UnitUnderTest
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> of the <see cref="UnitUnderTest"/></typeparam>
    public class TestBaseFor<T>
    {
        protected internal T UnitUnderTest;

        [SetUp]
        protected virtual void CreateUnitUnderTest()
        {
            UnitUnderTest = AutoBuild.InstanceByMakingUpParameters<T>(this);
        }
    }
}
