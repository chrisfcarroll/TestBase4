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
        /// The implementing subclass should attempt to find a concrete type, assignable to <paramref name="inOrderToBuildTypes"/> by
        /// following the rule which it (the subclass) names.
        /// </summary>
        /// <param name="type">The abstract type for which we seek a concrete subtype.</param>
        /// <param name="inOrderToBuildTypes">The type we are ultimately trying to build, the 
        ///     attempt to build which has recursively required an instance of <paramref name="type"/></param>
        /// <returns>A concrete <see cref="Type"/> which is assignable to <see cref="type"/>.
        /// Returns null if the rule identifies no suitable <see cref="Type"/>.</returns>
        public abstract Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes);
    }
}