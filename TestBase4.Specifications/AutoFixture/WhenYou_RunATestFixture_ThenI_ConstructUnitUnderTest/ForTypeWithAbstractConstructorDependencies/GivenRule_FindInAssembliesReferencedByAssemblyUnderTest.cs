using System.Reflection;
using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture]
	[FindInAssembliesReferencedByAssemblyUnderTest]
    public class GivenRule_FindInAssembliesReferencedByAssemblyUnderTest : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInReferencedAssemblies()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>());
        }
    }
}
