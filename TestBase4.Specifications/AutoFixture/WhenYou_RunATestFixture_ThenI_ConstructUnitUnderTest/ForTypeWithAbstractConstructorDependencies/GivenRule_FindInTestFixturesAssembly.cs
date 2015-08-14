using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture]
	[FindInTestFixturesAssembly]
    class GivenRule_FindInTestFixturesAssembly : TestBaseFor<ClassWith1ConstructorParam<INterface>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInSameAssembly()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterface>>());
            Assert.AreEqual(typeof(NterfaceImplementedInTestAssembly), UnitUnderTest.Param1.GetType());
        }
    }

    class NterfaceImplementedInTestAssembly : INterface {}
}