using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class AutoBuild
    {
        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the <see cref="IAutoBuildRule"/> rules
        /// found in the <see cref="Attribute"/>s of <paramref name="originalRequestor"/>.<see cref="object.GetType"/>
        /// </summary>
        /// <param name="originalRequestor">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceOf<T>(object originalRequestor)
        {
            return (T)InstanceOf(typeof(T), originalRequestor.GetType().GetAutoFixtureRulesFromAttributes(), typesWaitingToBeBuilt:null, originalRequestor: originalRequestor);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>originalRequestor</code> parameter, rules such as 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/> which depend on that parameter will not run.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rules"><see cref="IAutoBuildRule"/>s include rules for where to look for instantiable types and which constructor to use
        /// If <paramref name="rules"/> is null, the <see cref="DefaultRules"/> will be used.
        /// </param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceOf<T>(IEnumerable<IAutoBuildRule> rules= null)
        {
            return (T)InstanceOf(typeof(T), rules, null);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given  <paramref name="rules"/>.
        /// </summary>
        /// <param name="rules"><see cref="IAutoBuildRule"/> rules include rules for where to look for types to use as constructor
        /// parameters</param>
        /// <param name="originalRequestor">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceOf<T>(IEnumerable<IAutoBuildRule> rules, object originalRequestor)
        {
            return (T) InstanceOf(typeof (T), rules, typesWaitingToBeBuilt:null, originalRequestor: originalRequestor);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <paramref name="type"/>. Does so using the given <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>originalRequestor</code> parameter, rules such as 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/> which depend on that parameter will not run.
        /// </summary>
        /// <param name="type">The type of which a concrete instance is wanted.</param>
        /// <param name="rules"><see cref="IAutoBuildRule"/>s include rules for where to look for instantiable types and which constructor to use
        /// If <paramref name="rules"/> is null, the <see cref="DefaultRules"/> will be used.
        /// </param>
        /// <param name="typesWaitingToBeBuilt">The 'stack' of types we are trying to build grows as instantiating a type 
        /// recursively requires the instantion of its constructor dependencies.
        /// This parameter is for the benefit of build rules whose strategy may vary depending an what we're trying to build.
        /// </param>
        /// <param name="originalRequestor">The object which raised the original request to instantiate something. 
        /// This parameter is for the benefit of build rules whose strategy may vary depending on what we're trying to build. 
        /// For instance, <see cref="FindInTestFixturesAssemblyAttribute"/> takes <paramref name="originalRequestor"/> to be the TestFixture
        /// </param>
        /// <returns>An instance of type <paramref name="type"/> if possible, null if unable to construct one.</returns>
        public static object InstanceOf(Type type, IEnumerable<IAutoBuildRule> rules=null, IEnumerable<Type> typesWaitingToBeBuilt = null, object originalRequestor = null)
        {
            rules = rules ?? DefaultRules;
            typesWaitingToBeBuilt = (typesWaitingToBeBuilt ?? new List<Type>()).Union(new[] { type });

            var customRuleResult=rules.OfType<IAutoBuildCustomCreateRule>()
                .Select(r => r.CreateInstance(type, typesWaitingToBeBuilt, originalRequestor))
                .FirstOrDefault();

            if(customRuleResult!=null)
            {
                return customRuleResult;
            }
            else if (type.IsAbstract || type.IsInterface)
            {
                return InstanceOf(TypeFinder.FindConcreteTypeAssignableTo(type, rules, typesWaitingToBeBuilt, originalRequestor), rules, typesWaitingToBeBuilt, originalRequestor);
            }
            else if (type == typeof(string))
            {
                return typeof(string).Name;
            }
            else if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return InstanceFromConstructorRules(type, rules, typesWaitingToBeBuilt, originalRequestor);
            }
        }

        protected static object InstanceFromConstructorRules(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor)
        {
            var constructor = ChooseConstructor(type, rules.OfType<IAutoBuildChooseConstructorRule>() , typesWaitingToBeBuilt, originalRequestor);

            if (constructor == null || constructor.GetParameters().Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var pars = constructor.GetParameters()
                    .Select(p => InstanceOf(p.ParameterType, rules, typesWaitingToBeBuilt, originalRequestor))
                    .ToArray();
                return Activator.CreateInstance(type, pars);
            }
        }

        protected internal static ConstructorInfo ChooseConstructor(Type type, IEnumerable<IAutoBuildChooseConstructorRule> rules, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor)
        {
            return rules
                .Union(new[] {new ChooseConstructorWithFewestParametersAttribute()})
                .Select(r => r.ChooseConstructor(type, typesWaitingToBeBuilt, originalRequestor))
                .FirstOrDefault();
        }

        /// <summary>
        /// Identical to <see cref="DefaultRulesAttribute.AllDefaultRules"/>
        /// </summary>
        public static readonly IEnumerable<IAutoBuildRule> DefaultRules = DefaultRulesAttribute.AllDefaultRules;
    }
}