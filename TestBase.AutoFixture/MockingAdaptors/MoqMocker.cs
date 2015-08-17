using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class MoqMocker : IMockingLibraryBasicAdapter, IMockingLibraryInspectionAdapter
    {
        public object CreateMockElseNull(Type type, params object[] mockConstructorArgs)
        {
            try { return CreateMockElseThrow(type, mockConstructorArgs); }catch{ return null; }
        }

        public object CreateMockElseThrow(Type type, params object[] mockConstructorArgs)
        {
            var requiredMockType = MockerType?.MakeGenericType(type);
            var mockedObjectProperty = requiredMockType?.GetProperty("Object", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            var mock = Activator.CreateInstance(requiredMockType, mockConstructorArgs);
            return mockedObjectProperty.GetValue(mock);
        }

        public void EnsureMockingAssemblyIsLoadedAndWorkingElseThrow()
        {
            if(!IsMockingAssemblyFound())
            {
                throw new FileNotFoundException(
                    $"Unable to find a Moq.dll having a Moq.Mock`1 Type in directory {AppDomain.CurrentDomain.BaseDirectory}. " +
                    "Moq is most easily loaded by added a NuGet dependency to it from your Test project. ",
                    "Moq.dll");
            }
            CreateMockElseThrow(typeof(ICloneable /*an arbitrary example type that, if all is well, we will successfully mock.*/ ));
        }

        public bool IsMockingAssemblyFound() { return MockerType != null; }

        public bool IsThisMyMockObject(object value) { return GetMock(value) != null; }

        public object GetMock(object value)
        {
            return (mockType = mockType ?? new FindInAssemblyAttribute("Moq").FindTypeAssignableTo("Moq.Mock"))
                        ?.GetMethod("Get")
                        .Invoke(null, new[] { value });
        }

        public static Type MockerType { get { return mockerType = mockerType ?? new FindInAssemblyAttribute("Moq").FindTypeAssignableTo("Moq.Mock`1"); } }

        static Type mockerType;
        static Type mockType;
    }
}