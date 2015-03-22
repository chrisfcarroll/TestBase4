using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    /// <summary>
    /// <strong>Note</strong> that this strategy may consider several assemblies, not just one:
    /// <list type="bullet">
    /// <item>The 'Assembly Under Test' is primarily the assembly containing the Type under test.</item>
    /// <item>But there are also the assemblies containing the Types which we recursively need in 
    /// under to construct the Type under test.</item>
    /// </list>
    /// </summary>
    public class FindInAssemblyUnderTestAttribute : AutoFixtureStrategyAttribute 
    {
        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes)
        {
            return type
                    .Assembly.GetTypes()
                    .Union(inOrderToBuildTypes.SelectMany(t=>t.Assembly.GetTypes()))
                    .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
    }
}