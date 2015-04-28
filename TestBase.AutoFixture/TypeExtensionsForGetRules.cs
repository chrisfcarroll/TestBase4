using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    public static class TypeExtensionsForGetRules
    {
        public static IEnumerable<AutoFixtureStrategyAttribute> GetAutoFixtureRulesFromAttributes(this Type typeWithAttributes)
        {
            return typeWithAttributes
                .GetCustomAttributes(typeof(AutoFixtureStrategyAttribute), inherit: true)
                .Cast<AutoFixtureStrategyAttribute>();
        }
    }
}