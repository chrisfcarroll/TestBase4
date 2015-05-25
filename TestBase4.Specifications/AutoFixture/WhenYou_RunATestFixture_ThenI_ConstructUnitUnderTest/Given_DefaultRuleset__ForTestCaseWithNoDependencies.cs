using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoFixture.WhenYou_RunATestFixture_ThenI_ConstructUnitUnderTest
{
    [TestFixture, DefaultRules]
    class Given_DefaultRuleset__ForTestCaseWithNoDependencies : TestBaseFor<ClassWithDefaultConstructor>
    {
        [Test]
        public void AndI_BuildRequestedType()
        {
            Assert.That(UnitUnderTest, Is.Not.Null);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWithDefaultConstructor>());
        }
    }
}
