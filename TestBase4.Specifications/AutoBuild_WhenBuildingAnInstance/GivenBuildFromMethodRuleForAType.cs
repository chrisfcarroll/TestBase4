using System;
using System.Linq;
using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    [TestFixture]
    public class GivenBuildFromMethodRuleForAType
    {
        [Test]
        public void ThenI_UseIt_WhenBuilding()
        {
            const string aCustomString = "ACustomString";
            var result =
                AutoBuild.InstanceOf<AClass>(
                    DefaultRulesAttribute.AllDefaultRules
                        .Union(
                                new[] { new BuildFromFactoryAttribute(typeof(AClass),typeof(AFactory),"BuildMethod", aCustomString) }
                        ));
            //
            Assert.AreEqual(aCustomString, result.Aparameter);
        }

        [Test]
        public void Then_BuildFromFactoryAttribute_ThrowsFromConstructor_GivenFactoryClassAndMissingMethod()
        {
            Assert.Throws<InvalidOperationException>(() =>
                                                     {
                                                         new BuildFromFactoryAttribute(typeof(AClass), typeof(AFactory), "BuildMethodNameWhichDoesntExist");
                                                     });
        }

        [Test]
        public void ThenI_Throw_GivenRequestorObjectAndMissingMethod()
        {
            var rules = new[] {new BuildFromFactoryAttribute(typeof(AClass), "BuildMethodNameWhichDoesntExist")};
            //
            Assert.Throws<InvalidOperationException>(
                () => AutoBuild.InstanceOf<AClass>(DefaultRulesAttribute.AllDefaultRules.Union(rules))
                );
            Assert.Throws<InvalidOperationException>(
                () => AutoBuild.InstanceOf<AClass>(DefaultRulesAttribute.AllDefaultRules.Union(rules),this)
                );
        }


        class AClass {
            public readonly string Aparameter;
            public AClass(string aparameter) { Aparameter = aparameter; }
        }

        class AFactory
        {
            public AClass BuildMethod(string aparameter) { return new AClass(aparameter); }
        }
    }
}