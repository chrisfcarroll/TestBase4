using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TestBase
{
    public class AutoBuild
    {
        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using <see cref="AutoFixtureStrategyAttribute"/> rules
        /// found on <paramref name="requestedBy"/>.<see cref="object.GetType"/>
        /// </summary>
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(object requestedBy)
        {
            return (T)InstanceByMakingUpParameters(typeof(T), requestedBy.GetType().GetAutoFixtureRulesFromAttributes(), requestedBy);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given  <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>requestedBy</code> parameter, rules such as 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/> which depend on that parameter will not run.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rules"><see cref="AutoFixtureStrategyAttribute"/> rules include rules for where to look for types to use as constructor
        /// parameters</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(IEnumerable<AutoFixtureStrategyAttribute> rules)
        {
            return (T)InstanceByMakingUpParameters(typeof(T), rules, null);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given  <paramref name="rules"/>.
        /// </summary>
        /// <param name="rules"><see cref="AutoFixtureStrategyAttribute"/> rules include rules for where to look for types to use as constructor
        /// parameters</param>
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(IEnumerable<AutoFixtureStrategyAttribute> rules, object requestedBy)
        {
            return (T) InstanceByMakingUpParameters(typeof (T), rules, requestedBy);
        }


        protected static object InstanceByMakingUpParameters(Type type, IEnumerable<AutoFixtureStrategyAttribute> rules, object requestedBy, IEnumerable<Type> inOrderToBuildTypes = null)
        {
            inOrderToBuildTypes = (inOrderToBuildTypes ?? new List<Type>()).Union(new[] { type });
            var constructor = type.GetConstructors().FirstOrDefault();
            if (type == typeof(string))
            {
                return typeof(string).Name;
            }
            else if (type.IsAbstract || type.IsInterface)
            {
                return InstanceByMakingUpParameters(FindConcreteTypeAssignableTo(type, rules, inOrderToBuildTypes, requestedBy), rules, requestedBy, inOrderToBuildTypes);
            }
            else if (type.IsValueType || constructor == null || constructor.GetParameters().Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var pars = constructor.GetParameters()
                    .Select(p => InstanceByMakingUpParameters(p.ParameterType, rules, requestedBy, inOrderToBuildTypes))
                    .ToArray();
                return Activator.CreateInstance(type, pars);
            }
        }

        public static Type FindConcreteTypeAssignableTo(Type type, IEnumerable<AutoFixtureStrategyAttribute> rules, IEnumerable<Type> inOrderToBuildTypes, object requestedBy)
        {
            var result = rules
                .Select(r => r.FindTypeAssignableTo(type, inOrderToBuildTypes, requestedBy.GetType()))
                .FirstOrDefault(t => t != null);

            Assert.NotNull(
                result,
                "Failed to find a Type assignable to " + type.FullName + " using rules "
                + String.Join(", ",
                    rules
                        .ToArray().Select(r => r.GetType().Name)));
            return result;
        }
    }
}