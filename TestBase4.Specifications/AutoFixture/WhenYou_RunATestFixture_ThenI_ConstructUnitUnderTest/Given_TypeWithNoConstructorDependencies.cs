using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.TestData;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest
{
    [TestFixture]
    class Given_TypeWithNoConstructorDependencies : TestBaseFor<ClassWithDefaultConstructor>
    {
        [Test]
        public void ThenI_SetUnitUnderTestToAnInstanceOfT()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWithDefaultConstructor>());
        }

    }
}