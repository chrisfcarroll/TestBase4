using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
    public class AutoBuild
    {
        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using <see cref="AutoBuildFindTypeRuleAttribute"/> rules
        /// found on <paramref name="requestedBy"/>.<see cref="object.GetType"/>
        /// </summary>
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(object requestedBy)
        {
            return (T)InstanceByMakingUpParameters(typeof(T), requestedBy.GetType().GetAutoFixtureRulesFromAttributes(), inOrderToBuildTypes:null, requestedBy: requestedBy);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given  <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>requestedBy</code> parameter, rules such as 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/> which depend on that parameter will not run.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rules"><see cref="AutoBuildFindTypeRuleAttribute"/> rules include rules for where to look for types to use as constructor
        /// parameters</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(IEnumerable<IAutoBuildRule> rules)
        {
            return (T)InstanceByMakingUpParameters(typeof(T), rules, null);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given  <paramref name="rules"/>.
        /// </summary>
        /// <param name="rules"><see cref="IAutoBuildRule"/> rules include rules for where to look for types to use as constructor
        /// parameters</param>
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceByMakingUpParameters<T>(IEnumerable<IAutoBuildRule> rules, object requestedBy)
        {
            return (T) InstanceByMakingUpParameters(typeof (T), rules, inOrderToBuildTypes:null, requestedBy: requestedBy);
        }

        protected static object InstanceByMakingUpParameters(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> inOrderToBuildTypes = null, object requestedBy=null)
        {
            inOrderToBuildTypes = (inOrderToBuildTypes ?? new List<Type>()).Union(new[] { type });

            if (type == typeof(string))
            {
                return typeof(string).Name;
            }
            else if (type.IsAbstract || type.IsInterface)
            {
                return InstanceByMakingUpParameters(TypeFinder.FindConcreteTypeAssignableTo(type, rules, inOrderToBuildTypes, requestedBy), rules, inOrderToBuildTypes, requestedBy);
            }
            else if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return InstanceFromConstructorRules(type, rules, inOrderToBuildTypes, requestedBy);
            }
        }

        static object InstanceFromConstructorRules(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> inOrderToBuildTypes, object requestedBy)
        {
            var constructor = rules
                .OfType<AutoBuildChooseConstructorRuleAttribute>()
                .Union(new[] {new ChooseConstructorWithFewestParametersAttribute()})
                .Select(r => r.ChooseConstructor(type, inOrderToBuildTypes, requestedBy))
                .FirstOrDefault();

            if (constructor == null || constructor.GetParameters().Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var pars = constructor.GetParameters()
                    .Select(p => InstanceByMakingUpParameters(p.ParameterType, rules, inOrderToBuildTypes, requestedBy))
                    .ToArray();
                return Activator.CreateInstance(type, pars);
            }
        }
    }
}