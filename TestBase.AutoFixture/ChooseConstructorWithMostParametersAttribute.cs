using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class ChooseConstructorWithMostParametersAttribute : AutoBuildChooseConstructorRuleAttribute
    {

        public bool PreferPublic { get; set; }

        public override ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor=null)
        {
            return type.GetConstructors()
                .OrderByDescending(c => PreferPublic && c.IsPublic)
                .ThenByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
        }
    }
}