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
        public void ThenI_UseIt_WhenBuildingAString()
        {
            var result = AutoBuild.InstanceOf<string>(DefaultRulesAttribute.AllDefaultRules.Union( new []{ new CustomCreateRuleFor<string>("ACustomString") }  ));
            //
            Assert.AreEqual("ACustomString", result);
        }

        [Test]
        public void ThenI_UseIt_WhenBuildingAClass()
        {
            var customObject = new AClass();
            var result = AutoBuild.InstanceOf<AClass>(DefaultRulesAttribute.AllDefaultRules.Union( new []{ new CustomCreateRuleFor<AClass>(customObject) }  ));
            //
            Assert.AreSame(customObject, result);
        }

        class CustomCreateRuleFor<T> : TestBase.IAutoBuildCustomCreateRule
        {
            public readonly T Value;

            public CustomCreateRuleFor(T value) { Value=value;}

            public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null)
            {
                return Value;   
            }
        } 

        class AClass {}
    }
    
}
