using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class BuildFromMockAttribute : Attribute, IAutoBuildCustomCreateRule
    {
        public BuildFromMockAttribute(Type typeToMock, params object[] mockConstructorArgs)
        {
            this.typeToMock = typeToMock;
            this.knownMockingFrameworkAdapters = new IAutoBuildCustomCreateRule[]{ new BuildFromMoqAttribute(typeToMock, mockConstructorArgs) };
        }

        public BuildFromMockAttribute(params Type[] typesToMock) { }

        public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy)
        {
            if(type != typeToMock) { return null;}
            //
            return
                knownMockingFrameworkAdapters
                    .Select(a => a.CreateInstance(type, theStackOfTypesToBuild, requestedBy))
                    .FirstOrDefault();
        }

        readonly IAutoBuildCustomCreateRule[] knownMockingFrameworkAdapters;
        readonly Type typeToMock;
    }

    public class BuildFromMoqAttribute : Attribute, IAutoBuildCustomCreateRule
    {
        public BuildFromMoqAttribute(Type typeToMock, params object[] mockConstructorArgs)
        {
            this.typeToMock = typeToMock;
            this.mockConstructorArgs = mockConstructorArgs;
        }

        public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy)
        {
            if(type != typeToMock) { return null;}
            //
            var requiredMockType = MockerType?.MakeGenericType(type);
            var mockObjectProp = requiredMockType?.GetProperty("Object", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if(mockObjectProp == null) { return null; }
            //
            try
            {
                var mock = Activator.CreateInstance(requiredMockType, mockConstructorArgs );
                return mockObjectProp.GetValue(mock);
            } catch {
                return null;
            }
        }

        readonly Type typeToMock;
        readonly object[] mockConstructorArgs;

        public static object GetMoqMock(object value)
        {
            return MockType?.GetMethod("Get").Invoke(null, new[] { value });
        }

        /// <summary>
        /// The Type with name Moq.Mock`1 from which we can create a Mock&lt;T&gt;
        /// </summary>
        protected static Type MockerType
        {
            get { return mockerType = mockerType ?? new FindInAssemblyAttribute("Moq").FindTypeAssignableTo("Moq.Mock`1"); }
        }

        /// <summary>
        /// The Type with name Moq.Mock from which we can invoke Moq.Mock.Get(object)
        /// </summary>
        protected static Type MockType { get { return mockType = mockType ?? new FindInAssemblyAttribute("Moq").FindTypeAssignableTo("Moq.Mock"); } }

        static Type mockerType;
        static Type mockType;
    }
}