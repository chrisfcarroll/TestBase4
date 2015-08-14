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
            return FindTypeAssignableTo(testFixtureType, t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }

        public override Type FindTypeAssignableTo(string typeNameRightPart, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null)
        {
            return FindTypeAssignableTo(requestedBy, t => !t.IsAbstract && !t.IsInterface && t.FullName.EndsWith(typeNameRightPart));
        }

        static Type FindTypeAssignableTo(object testFixtureType, Func<Type, bool> filterBy)
        {
            return testFixtureType?.GetType().Assembly.GetTypes().FirstOrDefault(filterBy);
        }
    }
}