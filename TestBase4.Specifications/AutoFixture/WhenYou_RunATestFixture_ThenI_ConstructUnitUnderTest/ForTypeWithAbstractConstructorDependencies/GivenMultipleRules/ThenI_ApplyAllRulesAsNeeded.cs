﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies.GivenMultipleRules
{
    [TestFixture]
    [FindInAssemblyUnderTest]
    [FindInTestFixturesAssembly]
    [FindInAssembliesInBaseDirectory]
    class ThenI_ApplyAllRulesAsNeeded : TestBaseFor<ClassWith3ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly>>
    {
        [Test]
        public void AndI_BuildRequestedType()
        {
            Assert.That(UnitUnderTest, Is.Not.Null);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith3ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly>>());
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

    }

}
