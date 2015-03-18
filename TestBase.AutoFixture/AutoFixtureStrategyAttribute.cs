using System;

namespace TestBase
{
    public abstract class AutoFixtureStrategyAttribute : Attribute
    {
        public abstract Type FindTypeAssignableTo(Type type);
    }
}