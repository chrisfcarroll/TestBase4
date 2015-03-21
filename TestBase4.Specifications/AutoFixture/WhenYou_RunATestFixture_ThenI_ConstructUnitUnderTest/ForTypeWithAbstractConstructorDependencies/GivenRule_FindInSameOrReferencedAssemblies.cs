using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.AReferencedAssembly.TestData;
using TestBase4.Specifications.ReferencedAssembly2;
using TestBase4.Specifications.TestData;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture, FindInSameOrReferencedAssemblies]
    class GivenRule_FindInSameOrReferencedAssemblies : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInReferencedAssemblies()
        {
            //need to get types to load without this explicit forcing
            var loaded = new ReferencedAssembly2.NterfaceWithClassInReferencedAssembly();
            //
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterfaceWithClassInReferencedAssembly>>());
        }
    }
}