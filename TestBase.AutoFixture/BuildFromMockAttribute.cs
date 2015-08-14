using System;
using System.Collections.Generic;

namespace TestBase
{
    public class BuildFromMockAttribute : Attribute, IAutoBuildCustomCreateRule
    {
        readonly Type[] typesToMock;
        readonly Type typeToMock;
        readonly object[] args;

        public BuildFromMockAttribute(Type typeToMock,  params object[] args)
        {
            this.typeToMock = typeToMock;
            this.args = args;
        }
        public BuildFromMockAttribute(params Type[] typesToMock) { this.typesToMock = typesToMock; }

        public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null)
        {
            throw new NotImplementedException();
        }
    }
}