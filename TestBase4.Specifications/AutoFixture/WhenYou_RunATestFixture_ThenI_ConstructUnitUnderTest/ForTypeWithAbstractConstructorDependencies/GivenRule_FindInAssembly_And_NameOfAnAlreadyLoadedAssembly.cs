using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;
using System;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{

    [TestFixture]
	[FindInAssembly("TestBase4.Specifications")]
    class GivenRule_FindInAssembly_And_NameOfAnAlreadyLoadedAssembly : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithFakeInTestAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInNamedAssembly()
        {
            Assert.IsNotNull(UnitUnderTest);

            Assert.That(UnitUnderTest, 
                Is.AssignableTo
                <ClassWith1ConstructorParam<INterfaceWithFakeInTestAssembly>>());
            Assert.AreEqual(typeof(NterfaceWithFakeInTestAssembly), UnitUnderTest.Param1.GetType());
        }
    }

    class NterfaceWithFakeInTestAssembly : INterfaceWithFakeInTestAssembly {}
}