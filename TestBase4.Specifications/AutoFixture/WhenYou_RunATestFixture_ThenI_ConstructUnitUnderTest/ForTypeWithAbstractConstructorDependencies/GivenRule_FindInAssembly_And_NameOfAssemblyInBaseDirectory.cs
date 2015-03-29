using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;
using System;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest.ForTypeWithAbstractConstructorDependencies
{
    [TestFixture, FindInAssembly("TestBase4.TestCases.ANotReferencedAssembly")]
    class GivenRule_FindInAssembly_And_NameOfAssemblyInBaseDirectory : TestBaseFor<ClassWith1ConstructorParam<INterfaceWithClassInNotReferencedAssembly>>
    {
        [Test]
        public void ThenI_FindConcreteTypeForInterfaceInNamedAssembly()
        {
            Assert.IsNotNull(UnitUnderTest);

            Assert.That(UnitUnderTest, 
                    Is.AssignableTo
                        <ClassWith1ConstructorParam<INterfaceWithClassInNotReferencedAssembly>>());
            Assert.That(UnitUnderTest
                    .Param1.GetType()
                        .Assembly.FullName
                            .StartsWith("TestBase4.TestCases.ANotReferencedAssembly",StringComparison.InvariantCulture));
        }
    }
    
}