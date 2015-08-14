using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture]
	[FindInAssembliesInBaseDirectory]
    class GivenRule_FindInAssembliesInBaseDirectory : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInNotReferencedAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInAssembliesInBaseDirectory()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterfaceWithClassInNotReferencedAssembly>>());
        }

        [Test]
        public void ThenI_FindConcreteTypeEvenIfTheAssemblyIsntReferenced()
        {
            Assert.That(UnitUnderTest.Param1.GetType().Assembly.FullName.Contains("TestBase4.TestCases.ANotReferencedAssembly"));
        }
    }
}