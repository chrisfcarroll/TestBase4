using System;
using System.Collections.Generic;
using System.Reflection;


namespace TestBase
{
    public interface IAutoBuildRule { }

    public interface IAutoBuildCustomCreateRule : IAutoBuildRule
    {
        object CreateInstance(Type type, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor);
    }

    public interface IAutoBuildChooseConstructorRule : IAutoBuildRule
    {
        ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor = null);
    }

    public interface IAutoBuildFindTypeRule : IAutoBuildRule
    {
        Type FindTypeAssignableTo(Type type, IEnumerable<Type> typesWaitingToBeBuilt = null, object getType = null);
    }
    public interface IAutoBuildFindTypeByNameRule : IAutoBuildRule
    {
        Type FindTypeAssignableTo(string typeNameRightPart, IEnumerable<Type> typesWaitingToBeBuilt = null, object getType = null);
    }
}