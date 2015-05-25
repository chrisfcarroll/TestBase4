using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class AutoBuild
    {
        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the <see cref="IAutoBuildRule"/> rules
        /// found in the <see cref="Attribute"/>s of <paramref name="requestedBy"/>.<see cref="object.GetType"/>
        /// </summary>
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceOf<T>(object requestedBy)
        {
            return (T)InstanceOf(typeof(T), requestedBy.GetType().GetAutoFixtureRulesFromAttributes(), theStackOfTypesToBuild:null, requestedBy: requestedBy);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>requestedBy</code> parameter, rules such as 
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
        /// <param name="requestedBy">typically, you will pass in the TestFixture which is trying to build a concrete class. 
        /// The behaviour of some rules for finding concrete types—for instance <see cref="FindInTestFixturesAssemblyAttribute"/>—will be
        /// driven by the the type (or more likely the assembly) of this object.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        public static T InstanceOf<T>(IEnumerable<IAutoBuildRule> rules, object requestedBy)
        {
            return (T) InstanceOf(typeof (T), rules, theStackOfTypesToBuild:null, requestedBy: requestedBy);
        }

        /// <summary>
        /// AutoBuilds an instance of something assignable to <typeparamref name="T"/>. Does so using the given <paramref name="rules"/>.
        /// Note that since this overload doesn't take a <code>requestedBy</code> parameter, rules such as 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/> which depend on that parameter will not run.
        /// </summary>
        /// <param name="type">The type of which a concrete instance is wanted.</param>
        /// <param name="rules"><see cref="IAutoBuildRule"/>s include rules for where to look for instantiable types and which constructor to use
        /// If <paramref name="rules"/> is null, the <see cref="DefaultRules"/> will be used.
        /// </param>
        /// <param name="theStackOfTypesToBuild">The 'stack' of types we are trying to build grows as instantiating a type 
        /// recursively requires the instantion of its constructor dependencies.
        /// This parameter is for the benefit of build rules whose strategy may vary depending an what we're trying to build.
        /// </param>
        /// <param name="requestedBy">The object which raised the original request to instantiate something. 
        /// This parameter is for the benefit of build rules whose strategy may vary depending on what we're trying to build. 
        /// For instance, <see cref="FindInTestFixturesAssemblyAttribute"/> takes <paramref name="requestedBy"/> to be the TestFixture
        /// </param>
        /// <returns>An instance of type <paramref name="type"/> if possible, null if unable to construct one.</returns>
        public static object InstanceOf(Type type, IEnumerable<IAutoBuildRule> rules=null, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null)
        {
            rules = rules ?? DefaultRules;
            theStackOfTypesToBuild = (theStackOfTypesToBuild ?? new List<Type>()).Union(new[] { type });

            if (type == typeof(string))
            {
                return typeof(string).Name;
            }
            else if (type.IsAbstract || type.IsInterface)
            {
                return InstanceOf(TypeFinder.FindConcreteTypeAssignableTo(type, rules, theStackOfTypesToBuild, requestedBy), rules, theStackOfTypesToBuild, requestedBy);
            }
            else if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return InstanceFromConstructorRules(type, rules, theStackOfTypesToBuild, requestedBy);
            }
        }

        protected static object InstanceFromConstructorRules(Type type, IEnumerable<IAutoBuildRule> rules, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy)
        {
            var constructor = ChooseConstructor(type, rules.OfType<IAutoBuildChooseConstructorRule>() , theStackOfTypesToBuild, requestedBy);

            if (constructor == null || constructor.GetParameters().Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var pars = constructor.GetParameters()
                    .Select(p => InstanceOf(p.ParameterType, rules, theStackOfTypesToBuild, requestedBy))
                    .ToArray();
                return Activator.CreateInstance(type, pars);
            }
        }

        protected internal static ConstructorInfo ChooseConstructor(Type type, IEnumerable<IAutoBuildChooseConstructorRule> rules, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy)
        {
            return rules
                .Union(new[] {new ChooseConstructorWithFewestParametersAttribute()})
                .Select(r => r.ChooseConstructor(type, theStackOfTypesToBuild, requestedBy))
                .FirstOrDefault();
        }

        /// <summary>
        /// Identical to <see cref="DefaultRulesAttribute.AllDefaultRules"/>
        /// </summary>
        public static readonly IEnumerable<IAutoBuildRule> DefaultRules = DefaultRulesAttribute.AllDefaultRules;
    }
}