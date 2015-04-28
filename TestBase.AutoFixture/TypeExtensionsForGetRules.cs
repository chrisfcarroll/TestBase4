using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    public static class TypeExtensionsForGetRules
    {
        public static IEnumerable<AutoBuildFindTypeRuleAttribute> GetAutoFixtureRulesFromAttributes(this Type typeWithAttributes)
        {
            return typeWithAttributes
                .GetCustomAttributes(typeof(AutoBuildFindTypeRuleAttribute), inherit: true)
                .Cast<AutoBuildFindTypeRuleAttribute>();
        }
    }
}