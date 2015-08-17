using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    public static class TypeFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of which an instance (possibly of a subclass) is desired</param>
        /// <param name="rules">The <see cref="IAutoBuildRule"/> rules for where to search for Types</param>
        /// <param name="typesWaitingToBeBuilt"></param>
        /// <param name="originalRequestor"></param>
        /// <returns>A <see cref="Type"/>, if one is founnd or null if not.</returns>
        public static Type FindConcreteTypeAssignableTo(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor)
        {
            var result = rules
                .OfType<IAutoBuildFindTypeRule>()
                .Select(r => r.FindTypeAssignableTo(type, typesWaitingToBeBuilt, originalRequestor))
                .FirstOrDefault(t => t != null);
            return result;
        }
    }
}