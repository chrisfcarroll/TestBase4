using System;
using System.Collections.Generic;

namespace TestBase
{
    /// <summary>
    /// <para>Attributes inheriting from this class will be used as rules by instances of <see cref="TestBaseFor{T}"/>
    /// when constructing the <see cref="TestBaseFor{T}.UnitUnderTest"/> during the unit test setup phase.</para>
    /// 
    /// Rules are primarily concerned with 
    /// <list type="bullet">
    /// <item>Where to look (e.g. which assemblies or namespaces) to find a concrete type to construct 
    /// an instance of an interface or abstract type</item>
    /// </list>
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class,Inherited = true,AllowMultiple = true)]
    public abstract class AutoFixtureStrategyAttribute : Attribute
    {
        /// <summary>
        /// Implementing subclasses should attempt to find a concrete type, assignable to <paramref name="type"/> by
        /// following the rule which it (the subclass) names.
        /// </summary>
        /// <param name="type">The abstract Type for which we are now trying to build a concrete instance.</param>
        /// <param name="inOrderToBuildTypes">The type which we were ultimately trying to build, and the types
        ///     we need to build it, which has recursively led us to need an instance of <paramref name="type"/>.
        /// </param>
        /// <param name="testFixtureType">the Type of the test fixture which is trying to construct something</param>
        /// <returns>
        /// <list type="table">
        /// <item>A concrete <see cref="Type"/> which is assignable to <see cref="type"/>.</item>
        /// <item>Returns null if the rule can identify no suitable <see cref="Type"/>.</item>
        /// </list>
        /// </returns>
        public abstract Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes, Type testFixtureType=null);
    }
}