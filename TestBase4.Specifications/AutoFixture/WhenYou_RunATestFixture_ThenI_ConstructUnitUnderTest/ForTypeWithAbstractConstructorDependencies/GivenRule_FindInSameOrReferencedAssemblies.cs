using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.AReferencedAssembly.TestData;
using TestBase4.Specifications.ReferencedAssembly2;
using TestBase4.Specifications.TestData;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture, FindInAssemblyUnderTestOrReferencedAssemblies]
    class GivenRule_FindInSameOrReferencedAssemblies : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInReferencedAssemblies()
        {
            var forceReferenceToAssembly2 = new SomeOtherTypeInReferencedAssembly2();
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>());
        }
    }
}
