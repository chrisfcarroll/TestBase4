using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected internal object CreateInstanceByMakingUpParameters(Type type, IEnumerable<Type> inOrderToBuildTypes= null)
        {
            inOrderToBuildTypes = (inOrderToBuildTypes ?? new List<Type>()).Union(new []{this.GetType(), type});
            var constructor = type.GetConstructors().FirstOrDefault();
            if (type == typeof (string))
            {
                return typeof(string).Name;
            }
            else if (type.IsAbstract || type.IsInterface)
            {
                return CreateInstanceByMakingUpParameters(FindConcreteTypeAssignableTo(type, inOrderToBuildTypes));
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

        protected internal Type FindConcreteTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes)
        {
            var result=this.GetType()
                .GetCustomAttributes(typeof (AutoFixtureStrategyAttribute), inherit: true)
                .Cast<AutoFixtureStrategyAttribute>()
                .Select(r => r.FindTypeAssignableTo(type,inOrderToBuildTypes))
                .FirstOrDefault(t => t != null);

            Debug.Assert(result!=null,
                "Failed to find a Type assignable to " + type.FullName + " using rules " 
                    + string.Join(", ", 
                        this.GetType()
                            .GetCustomAttributes(typeof (AutoFixtureStrategyAttribute), inherit: true)
                            .Cast<AutoFixtureStrategyAttribute>()
                            .ToArray().Select(r=>r.GetType().Name)));
            return result;
        }
    }
}
