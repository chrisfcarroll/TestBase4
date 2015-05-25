using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class ChooseConstructorWithFewestParametersAttribute : AutoBuildChooseConstructorRuleAttribute
    {

        public bool PreferPublic { get; set; }

        public override ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy=null)
        {
            return type.GetConstructors()
                .OrderByDescending(c => PreferPublic && c.IsPublic)
                .ThenBy(c => c.GetParameters().Length)
                .FirstOrDefault();
        }
    }
}