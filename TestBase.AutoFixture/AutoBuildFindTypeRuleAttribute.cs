using System;
using System.Collections.Generic;

namespace TestBase
{
    /// <summary>
    /// <para>Attributes inheriting from this class will be used as rules by <see cref="AutoBuild"/>/>
    /// when constructing a concrete instance</para>
    /// 
    /// Rules inheriting from <see cref="AutoBuildFindTypeRuleAttribute"/> are concerned with 
    /// where to look (e.g. which assemblies or namespaces) for a concrete type
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class,Inherited = true,AllowMultiple = true)]
    public abstract class AutoBuildFindTypeRuleAttribute : Attribute, IAutoBuildFindTypeRule
    {
        /// <summary>
        /// Implementing subclasses should attempt to find a concrete type, assignable to <paramref name="type"/> by
        /// following the rule which it (the subclass) names.
        /// </summary>
        /// <param name="type">The abstract Type for which we are now trying to build a concrete instance.</param>
        /// <param name="theStackOfTypesToBuild">The type which we were ultimately trying to build, and the types
        ///     we need to build it, which has recursively led us to need an instance of <paramref name="type"/>.
        /// </param>
        /// <param name="requestedBy"></param>
        /// <returns>
        /// <list type="table">
        /// <item>A concrete <see cref="Type"/> which is assignable to <see cref="type"/>.</item>
        /// <item>Returns null if the rule can identify no suitable <see cref="Type"/>.</item>
        /// </list>
        /// </returns>
        public abstract Type FindTypeAssignableTo(Type type, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null);
    }
}