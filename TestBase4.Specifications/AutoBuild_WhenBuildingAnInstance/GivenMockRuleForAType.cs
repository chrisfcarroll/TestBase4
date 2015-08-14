using System.Linq;
using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    [TestFixture,Ignore("WIP Next todo")]
    public class GivenMockRuleForAType
    {
        [Test]
        public void ThenI_UseIt_WhenBuildingAClass()
        {
            var result =
                AutoBuild.InstanceOf<ClassWith1ConstructorParam<INterface>>(
                    DefaultRulesAttribute.AllDefaultRules
                    .Union(
                        new[] { new BuildFromMockAttribute(typeof(INterface)) }
                        ));
            //
            Assert.IsTrue(result.Param1.IsAMock());
        }
    }


    public static class MockHelper
    {
        public static bool IsAMock(this object value) { return IsAMoq(value) || IsAnNMock(value); }
        static bool IsAnNMock(object value) { throw new System.NotImplementedException(); }

        static bool IsAMoq(object value) { throw new System.NotImplementedException(); }
    }

}