using System;
using NUnit.Framework;
using TestBase;
using TestBase4.Specifications.AutoFixture.TestTypes;

namespace TestBase4.Specifications.AutoFixture.ForTypeWithAbstractConstructorDependencies
{
    public class WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest
    {
        [TestFixture, FindInSameAssembly]
        class GivenRule_FindInSameAssembly : TestBaseFor<ClassWith1ConstructorParam<INterface>>
        {
            [Test]
            public void ThenI_FindConcreteTypeForInterfaceInSameAssembly()
            {
                Assert.IsNotNull(UnitUnderTest);
                Assert.That(UnitUnderTest, Is.AssignableTo<ClassWith1ConstructorParam<INterface>>());
            }
        }
    }
}