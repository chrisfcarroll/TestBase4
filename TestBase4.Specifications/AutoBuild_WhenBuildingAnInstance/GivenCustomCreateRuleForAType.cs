using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    [TestFixture]
    public class GivenCustomCreateRuleForAType
    {
        [Test]
        public void ThenI_UseIt_WhenBuildingAClass()
        {
            var customObject = new AClass();
            var result =
                AutoBuild.InstanceOf<AClass>(DefaultRulesAttribute.AllDefaultRules.Union(new[] {new CustomCreateRuleFor<AClass>(customObject)}));
            //
            Assert.AreSame(customObject, result);
        }

        [Test]
        public void ThenI_UseIt_WhenBuildingAString()
        {
            var result =
                AutoBuild.InstanceOf<string>(DefaultRulesAttribute.AllDefaultRules.Union(new[] {new CustomCreateRuleFor<string>("ACustomString")}));
            //
            Assert.AreEqual("ACustomString", result);
        }

        class CustomCreateRuleFor<T> : IAutoBuildCustomCreateRule
        {
            public CustomCreateRuleFor(T value) { this.value = value; }
            public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null) { return value; }
            readonly T value;
        }

        class AClass { }
    }
}