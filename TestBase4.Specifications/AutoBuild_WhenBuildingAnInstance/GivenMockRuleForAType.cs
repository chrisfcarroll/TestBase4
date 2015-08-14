using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    public class PreRequisiteAttribute : TestAttribute { }

    [TestFixture]
    public class Given_BuildFromMockRule__And_Moqdll_IsFindable
    {
        [PreRequisite]
        public void ThereIsAMockingFrameworkInTheBaseDirectory()
        {
            var moq= new FindInAssemblyAttribute("Moq").FindTypeAssignableTo("Mock`1");
            //
            Assert.IsNotNull(moq,"Didn't find a known mock framework (i.e. Moq) in Base Directory, can't test mocking.");
        }

        [Test]
        public void ThenI_UseIt_WhenBuildingAClass()
        {
            var result =
                AutoBuild
                    .InstanceOf<ClassWith1ConstructorParam<INterface>>(
                        new[] { new BuildFromMockAttribute(typeof(INterface)) }
                        );
            //
            Assert.IsTrue(MockHelper.IsAMock(result.Param1));
        }
    }


    static class MockHelper
    {
        public static bool IsAMock(object value) { return knownIsMockTests.Select(m => m(value)).Any(); }
        static bool IsAMoq(object value) { return BuildFromMoqAttribute.GetMoqMock(value) != null; }

        static Func<object, bool>[] knownIsMockTests = { IsAMoq };
    }
}