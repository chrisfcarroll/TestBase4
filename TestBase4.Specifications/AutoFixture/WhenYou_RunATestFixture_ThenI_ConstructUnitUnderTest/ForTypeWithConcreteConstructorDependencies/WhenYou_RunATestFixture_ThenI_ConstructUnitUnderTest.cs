using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithConcreteConstructorDependencies
{
    [TestFixture]
    class Given_DependencyOnValueType : TestBaseFor<ClassWith1ConstructorParam<int>>
    {
        [Test]public void ForTypeWithDependencyOnValueType()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<int>>());
        }
    }

    [TestFixture]
    class Given_DependencyOnString : TestBaseFor<ClassWith1ConstructorParam<string>>
    {
        [Test]
        public void ForTypeWithDependencyOnString()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<string>>());
        }
    }

    [TestFixture]
    class Given_DependencyOnTypeWithItself1ConstructorDependency : TestBaseFor<ClassWith1ConstructorParam<ClassWith1ConstructorParam<string>>>
    {
        [Test]
        public void ForTypeWithDependencyOnTypeWithItself1ConstructorDependency()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<ClassWith1ConstructorParam<string>>>());
        }
    }
}