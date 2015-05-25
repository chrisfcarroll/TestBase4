using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// <strong>Note</strong> that this strategy may consider not one but several assemblies:
    /// <list type="bullet">
    /// <item>The 'Assembly Under Test' is primarily the assembly containing the Type under test.</item>
    /// <item>But there are also the assemblies containing the Types which we recursively need in 
    /// under to construct the Type under test.</item>
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