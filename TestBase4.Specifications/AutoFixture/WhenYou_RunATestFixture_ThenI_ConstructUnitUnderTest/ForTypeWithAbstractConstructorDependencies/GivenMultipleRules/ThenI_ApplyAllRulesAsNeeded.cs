using System;
using System.Collections.Generic;
using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies.GivenMultipleRules
{
    [TestFixture]
    [FindInAssemblyUnderTest]
    [FindInTestFixturesAssembly]
    [FindInAssembliesInBaseDirectory]
    [BuildFromMock(typeof(ICloneable))]
    class ThenI_ApplyAllRulesAsNeeded : TestBaseFor<ClassWith4ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly, ICloneable>>
    {
        [Test]
        public void AndI_BuildRequestedType()
        {
            Assert.That(UnitUnderTest, Is.Not.Null);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith4ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly, ICloneable>>());
        }

        [Test]
        public void AndI_FindConcreteTypeForINterfaceWithClassInSameAssembly()
        {
            Assert.That(UnitUnderTest.Param1, Is.AssignableTo<INterfaceWithClassInSameAssembly>());
        }

        [Test]
        public void AndI_FindConcreteTypeForINterfaceWithFakeInTestAssembly()
        {
            Assert.That(UnitUnderTest.Param2, Is.AssignableTo<INterfaceWithFakeInTestAssembly>());
            Assert.That(UnitUnderTest.Param2.GetType().Assembly, Is.EqualTo(this.GetType().Assembly));
        }

        [Test]
        public void AndI_FindConcreteTypeForInterfaceInAssembliesInBaseDirectoryEvenIfTheAssemblyIsntReferenced()
        {
            Assert.That(UnitUnderTest.Param3, Is.AssignableTo<INterfaceWithClassInNotReferencedAssembly>());
            Assert.That(UnitUnderTest.Param3.GetType().Assembly.FullName.Contains("TestBase4.TestCases.ANotReferencedAssembly"));
        }

        [Test,Ignore("WIP")]
        public void AndI_MockAnInterface__AssumingThatAKnownMockingFrameworkAssemblyWasFindable()
        {
            Assert.That(UnitUnderTest.Param4, Is.AssignableTo<ICloneable>());
            Assert.True(MockHelper.IsAMock(UnitUnderTest.Param4));
        }

    }

}
