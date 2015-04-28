using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TestBase
{
    public static class TypeFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of which an instance (possibly of a subclass) is desired</param>
        /// <param name="rules">The <see cref="AutoBuildFindTypeRuleAttribute"/> rules for where to search for Types</param>
        /// <param name="inOrderToBuildTypes"></param>
        /// <param name="requestedBy"></param>
        /// <returns>A <see cref="Type"/>, if one is founnd or null if not.</returns>
        public static Type FindConcreteTypeAssignableTo(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> inOrderToBuildTypes, object requestedBy)
        {
            var result = rules
                .OfType<AutoBuildFindTypeRuleAttribute>()
                .Select(r => r.FindTypeAssignableTo(type, inOrderToBuildTypes, requestedBy))
                .FirstOrDefault(t => t != null);
            return result;
        }
    }
}