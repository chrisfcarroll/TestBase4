using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    /// <summary>
    /// This strategy will look just in the Test Assembly, i.e. the Assembly in which the TestFixture is defined.
    /// </summary>
    public class FindInTestFixturesAssemblyAttribute : AutoFixtureStrategyAttribute 
    {
        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes, Type testFixtureType)
        {
            return testFixtureType
                    .Assembly.GetTypes()
                    .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
    }
}