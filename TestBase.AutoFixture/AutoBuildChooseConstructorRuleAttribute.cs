using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// <para>Attributes inheriting from this class will be used as rules by <see cref="AutoBuild"/>/>
    /// when constructing a concrete instance</para>
    /// 
    /// Rules inheriting from <see cref="AutoBuildChooseConstructorRuleAttribute"/> are concerned with 
    /// which constructor to choose (if there is more than one) when building a concrete type
    /// </summary>
    public abstract class AutoBuildChooseConstructorRuleAttribute : Attribute, IAutoBuildRule
    {
        public abstract ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> inOrderToBuildTypes, object requestedBy = null);
    }
}