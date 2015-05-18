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
    public class AutoBuildDefaultRulesAttribute : Attribute, IAutoBuildFindTypeRule, IAutoBuildChooseConstructorRule
    {
        public Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes = null, object requestedBy = null)
        {
            return TypeFinder.FindConcreteTypeAssignableTo(type, AllDefaultRules, inOrderToBuildTypes, requestedBy);
        }

        public ConstructorInfo ChooseConstructor(Type type, IEnumerable<Type> inOrderToBuildTypes, object requestedBy = null)
        {
            return AutoBuild.ChooseConstructor(type, DefaultChooseConstructorRules, inOrderToBuildTypes, requestedBy);
        }

        public static readonly IList<IAutoBuildFindTypeRule> 
                                   DefaultFindTypeRuleSequence = 
                                           new ReadOnlyCollection<IAutoBuildFindTypeRule>(
                                               new IAutoBuildFindTypeRule[]{ 
                                                   new FindInAssemblyUnderTestAttribute(), 
                                                   new FindInTestFixturesAssemblyAttribute(), 
                                                   new FindInAssembliesInBaseDirectoryAttribute() });

        public static readonly IList<IAutoBuildChooseConstructorRule>
                                   DefaultChooseConstructorRules =
                                           new ReadOnlyCollection<IAutoBuildChooseConstructorRule>(
                                               new IAutoBuildChooseConstructorRule[]{ 
                                                   new ChooseConstructorWithMostParametersAttribute(),
                                                   new ChooseConstructorWithFewestParametersAttribute() });

        public static readonly IEnumerable<IAutoBuildRule> 
                                    AllDefaultRules = 
                                        new ReadOnlyCollection<IAutoBuildRule>(
                                                (DefaultFindTypeRuleSequence.
                                                    Union<IAutoBuildRule>(
                                                        DefaultChooseConstructorRules)
                                                ).ToList());

    }
}