using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture, FindInAssemblyUnderTest]
    class GivenRule_FindInAssemblyUnderTest : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInSameAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInSameAssembly()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterfaceWithClassInSameAssembly>>());
        }
    }
}