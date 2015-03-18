using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.AutoFixture.TestTypes;

namespace TestBase4.Specifications.AutoFixture
{
    [TestFixture]
    class WhenYou_RunATextFixtureForTypeWithNoConstructorDependencies : TestBaseFor<ClassWithDefaultConstructor>
    {
        [Test]
        public void ThenI_SetUnitUnderTestToAnInstanceOfT()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWithDefaultConstructor>());
        }

    }
}