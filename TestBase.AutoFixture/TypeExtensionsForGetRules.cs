using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    public static class TypeExtensionsForGetRules
    {
        public static IEnumerable<IAutoBuildRule> GetAutoFixtureRulesFromAttributes(this Type typeWithAttributes)
        {
            return typeWithAttributes
                .GetCustomAttributes(typeof(IAutoBuildRule), inherit: true)
                .Cast<IAutoBuildRule>();
        }
    }
}