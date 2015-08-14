using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// This set of rules should do the right thing in many cases. It applies these rulesets: 
    /// <list type="bullet">
    /// <item>The rules in <see cref="DefaultFindTypeRuleSequence"/> in order, when looking for a concrete type to instantiate an interface or abstract type</item>
    /// <item>The rules in <see cref="DefaultChooseConstructorRuleSequence"/> when constructing a concrete type.</item>
    /// </list>
    /// </summary>
    public class DefaultRulesAttribute : Attribute, IAutoBuildFindTypeRule, IAutoBuildChooseConstructorRule
    {
        public Type FindTypeAssignableTo(Type type, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null)
        {
            return TypeFinder.FindConcreteTypeAssignableTo(type, AllDefaultRules, theStackOfTypesToBuild, requestedBy);
        }

        public ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> theStackOfTypesToBuild, object requestedBy = null)
        {
            return AutoBuild.ChooseConstructor(type, DefaultChooseConstructorRuleSequence, theStackOfTypesToBuild, requestedBy);
        }

        /// <summary>
        /// The default rule sequence for finding a type to instantiate is, in this order:
        /// <see cref="FindInAssemblyUnderTestAttribute"/>, 
        /// <see cref="FindInTestFixturesAssemblyAttribute"/>, 
        /// <see cref="FindInAssembliesInBaseDirectoryAttribute"/>
        /// </summary>
        public static readonly IList<IAutoBuildFindTypeRule> 
                                   DefaultFindTypeRuleSequence = 
                                           new ReadOnlyCollection<IAutoBuildFindTypeRule>(
                                               new IAutoBuildFindTypeRule[]{ 
                                                   new FindInAssemblyUnderTestAttribute(), 
                                                   new FindInTestFixturesAssemblyAttribute(), 
                                                   new FindInAssembliesInBaseDirectoryAttribute() });

        /// <summary>
        /// The default ConstructorRule is, in this order:
        /// <see cref="ChooseConstructorWithMostParametersAttribute"/>,
        /// <see cref="ChooseConstructorWithFewestParametersAttribute"/>
        /// </summary>
        public static readonly IList<IAutoBuildChooseConstructorRule>
                                   DefaultChooseConstructorRuleSequence =
                                           new ReadOnlyCollection<IAutoBuildChooseConstructorRule>(
                                               new IAutoBuildChooseConstructorRule[]{ 
                                                   new ChooseConstructorWithMostParametersAttribute(),
                                                   new ChooseConstructorWithFewestParametersAttribute() });

        /// <summary>
        /// The default Autobuild ruleset is the union of <see cref="DefaultFindTypeRuleSequence"/> and <see cref="DefaultChooseConstructorRuleSequence"/>
        /// </summary>
        public static readonly IEnumerable<IAutoBuildRule> 
                                    AllDefaultRules = 
                                        new ReadOnlyCollection<IAutoBuildRule>(
                                                (DefaultFindTypeRuleSequence.
                                                    Union<IAutoBuildRule>(
                                                        DefaultChooseConstructorRuleSequence)
                                                ).ToList());

    }
}