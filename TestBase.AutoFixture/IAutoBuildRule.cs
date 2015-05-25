using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestBase
{
    public interface IAutoBuildRule { }

    public interface IAutoBuildChooseConstructorRule : IAutoBuildRule
    {
        ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null);
    }

    public interface IAutoBuildFindTypeRule : IAutoBuildRule
    {
        Type FindTypeAssignableTo(Type type, IEnumerable<Type> theStackOfTypesToBuild = null, object getType = null);
    }
}