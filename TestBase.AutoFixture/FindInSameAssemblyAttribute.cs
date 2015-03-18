using System;
using System.Linq;

namespace TestBase
{
    public class FindInSameAssemblyAttribute : AutoFixtureStrategyAttribute 
    {
        public override Type FindTypeAssignableTo(Type type)
        {
            return type.Assembly
                .GetTypes()
                .FirstOrDefault(t=> t!=type && type.IsAssignableFrom(t));
        }
    }
}