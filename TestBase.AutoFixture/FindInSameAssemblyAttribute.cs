using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    /// <summary>
    /// This strategy may actually consider several assemblies, not just one:
    /// The assembly of the interface or abstract type we are trying to construct; and the 
    /// assembly of any of the types we have tried to construct and which led us to needing to construct this type.
    /// </summary>
    public class FindInSameAssemblyAttribute : AutoFixtureStrategyAttribute 
    {
        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes)
        {
            return type.Assembly
                    .GetTypes()
                    .Union(inOrderToBuildTypes.SelectMany(t=>t.Assembly.GetTypes()))
                    .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
    }
}