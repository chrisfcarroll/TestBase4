using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoFixture
{
    public class WhenYou_RunAFixtureTestingAClassWith1ConstructorParam
    {
        [TestFixture]class OfValueType : TestBaseFor<ClassWith1ConstructorParam<int>>
        {
            [Test]public void ThenI_SetUnitUnderTestToAnInstanceOfT()
            {
                Assert.IsNotNull(UnitUnderTest);
                Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<int>>());
            }
        }

        [TestFixture]
        class OfTypeWithItselfNoConstructorParams : TestBaseFor<ClassWith1ConstructorParam<string>>
        {
            [Test]
            public void ThenI_SetUnitUnderTestToAnInstanceOfT()
            {
                Assert.IsNotNull(UnitUnderTest);
                Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<string>>());
            }
        }

        [TestFixture]
        class OfTypeWithItself1Constructor : TestBaseFor<ClassWith1ConstructorParam<ClassWith1ConstructorParam<string>>>
        {
            [Test]
            public void ThenI_SetUnitUnderTestToAnInstanceOfT()
            {
                Assert.IsNotNull(UnitUnderTest);
                Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<ClassWith1ConstructorParam<string>>>());
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        class ClassWith1ConstructorParam<T>
        {
            readonly T param1;
            public ClassWith1ConstructorParam(T param1) { this.param1 = param1; }
        }
    }
}