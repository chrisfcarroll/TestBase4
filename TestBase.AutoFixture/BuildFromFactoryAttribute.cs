using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    public class BuildFromFactoryAttribute : Attribute, IAutoBuildCustomCreateRule
    {
        readonly Type targetTypeToBuild;
        readonly Type factoryClass;
        readonly string factoryMethodName;
        readonly object[] args;

        /// <summary>
        /// Specifies a specific factory method to call 
        /// </summary>
        /// <param name="targetTypeToBuild">The type, the autobuilding of which should be done by factory method</param>
        /// <param name="factoryClass">The class where the factory method can be found. 
        /// If <paramref name="factoryMethodName"/> is not static then it must be possible to Autobuild an instance (for instance, by calling a constructor with no, or autobuildable, parameters)</param>
        /// <param name="factoryMethodName">The factory method to call each time we try to create a <paramref name="targetTypeToBuild"/></param>
        /// <param name="args">any arguments needed for the factory method</param>
        public BuildFromFactoryAttribute(Type targetTypeToBuild, Type factoryClass, string factoryMethodName, params object[] args)
        {
            this.targetTypeToBuild = targetTypeToBuild;
            this.factoryClass = factoryClass;
            this.factoryMethodName = factoryMethodName;
            this.args = args;
            if(factoryClass != null){ EnsureFactoryMethodElseThrow(factoryClass,null); }
        }
        /// <summary>
        /// Specifies a factory method to call. The method should exist on the requestedBy object passed to <see cref="AutoBuild.InstanceOf"/> when building.
        /// In typical usage, the requestedBy object will be the TestFixture decorated with this instance of <see cref="BuildFromFactoryAttribute"/>
        /// </summary>
        /// <param name="targetTypeToBuild">The type, the autobuilding of which should be done by factory method</param>
        /// <param name="factoryMethodName">The factory method to call each time we try to create a <paramref name="targetTypeToBuild"/></param>
        /// <param name="args">any arguments needed for the factory method</param>
        public BuildFromFactoryAttribute(Type targetTypeToBuild, string factoryMethodName, params object[] args)
        {
            this.targetTypeToBuild = targetTypeToBuild;
            this.factoryMethodName = factoryMethodName;
            this.args = args;
            if (factoryClass != null) { EnsureFactoryMethodElseThrow(factoryClass, null); }
        }

        public object CreateInstance(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null)
        {
            if(type != targetTypeToBuild) {return null;}
            //
            if(factoryClass == null && requestedBy == null)
            {
                throw new InvalidOperationException("You called BuildFromFactoryAttribute.CreateInstance() with giving a System.Type for the Factory. Either specify a Type in the BuildFromFactory constructor, or ensure that a RequestedBy object is given in the call to AutoBuild.");
            }
            //
            object factory = requestedBy ?? AutoBuild.InstanceOf(factoryClass,DefaultRulesAttribute.DefaultFindTypeRuleSequence.Union<IAutoBuildRule>(new[] {new ChooseConstructorWithFewestParametersAttribute()})); //nb if the factory method is static, then it's okay for factory to be null.

            Type factoryClassToUse = factory?.GetType()??factoryClass;

            var m = EnsureFactoryMethodElseThrow(factoryClassToUse, requestedBy);
            //
            return m.Invoke(factory,args);
        }

        MethodInfo EnsureFactoryMethodElseThrow(Type factoryClassToUse, object requestedBy)
        {
            var m = factoryClassToUse.GetMethod(factoryMethodName);
            //
            if(m == null)
            {
                throw new InvalidOperationException(
                    string.Format(MethodNotFoundFormat,
                                  targetTypeToBuild,
                                  factoryClass,
                                  factoryMethodName,
                                  requestedBy,
                                  factoryClassToUse));
            }
            if(!m.ReturnType.IsAssignableFrom(targetTypeToBuild))
            {
                throw new ArgumentOutOfRangeException(nameof(targetTypeToBuild),
                                                      string.Format(ReturnTypeNotAssignableToTargetFormat,
                                                                    targetTypeToBuild,
                                                                    factoryClassToUse,
                                                                    factoryMethodName,
                                                                    m.ReturnType));
            }
            return m;
        }

        const string ReturnTypeNotAssignableToTargetFormat="BuildFromMethod({0},{1},{2}) doesn't work because {0} is not assignable to the return type {3} of {1}.{2}";
        const string MethodNotFoundFormat = "You asked for BuildFromFactory({0},{1},{2},...).CreateInstance(requestedBy:{3}) but there is no method {4}.{2}";
    }
}
