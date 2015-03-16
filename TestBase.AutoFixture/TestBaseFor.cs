using System;
using System.Linq;
using NUnit.Framework;

namespace TestBase
{
    /// <summary>
    /// A Base Class for TestFixtures / TestClasses which will auto-construct a UnitUnderTest
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> of the <see cref="UnitUnderTest"/></typeparam>
    public class TestBaseFor<T>
    {
        protected internal T UnitUnderTest;

        [SetUp]
        protected virtual void CreateUnitUnderTest()
        {
            UnitUnderTest = (T)CreateInstanceByMakingUpParameters(typeof(T));
        }

        protected internal object CreateInstanceByMakingUpParameters(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault();
            if (type == typeof (string))
            {
                return typeof(string).Name;
            }
            else if (type.IsValueType || constructor==null || constructor.GetParameters().Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var pars = constructor.GetParameters()
                            .Select(p => CreateInstanceByMakingUpParameters(p.ParameterType))
                            .ToArray();
                return Activator.CreateInstance(type, pars);
            }
        }
    }
}
