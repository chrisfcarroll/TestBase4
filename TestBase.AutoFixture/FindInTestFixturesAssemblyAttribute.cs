using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    /// <summary>
    /// This strategy will look just in the Test Assembly, i.e. the Assembly in which the TestFixture is defined.
    /// </summary>
    public class FindInTestFixturesAssemblyAttribute : AutoBuildFindTypeRuleAttribute 
    {
        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> theStackOfTypesToBuild = null, object testFixtureType = null)
        {
            return testFixtureType==null
                    ? null
                    : testFixtureType.GetType()
                        .Assembly.GetTypes()
                        .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
    }
}