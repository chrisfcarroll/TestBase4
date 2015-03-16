using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoFixture
{
    [TestFixture]
    class WhenYou_RunAFixtureTestingAClassWithNoConstructorParameters : TestBaseFor<WhenYou_RunAFixtureTestingAClassWithNoConstructorParameters.ClassWithDefaultConstructor>
    {
        [Test]
        public void ThenI_SetUnitUnderTestToAnInstanceOfT()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWithDefaultConstructor>());
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        internal class ClassWithDefaultConstructor { }
    }
}