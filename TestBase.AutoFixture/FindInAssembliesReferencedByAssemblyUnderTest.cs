using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// <para><strong>Warning:</strong> "Referenced Assembly" -- as in <see cref="Assembly.GetReferencedAssemblies"/> --
    /// may not mean what you think. Adding an assembly as a reference to a project does not make 
    /// it a referenced assembly. Only if your code actually refers to a Type from that assembly 
    /// does it become referenced.</para>
    /// 
    /// <strong>Note</strong> that this strategy will consider the referenced assemblies of not one but several assemblies:
    /// <list type="bullet">
    /// <item>The assemblies referenced by the 'Assembly Under Test', that is the assembly containing the Type under test.</item>
    /// <item>Then there are the assemblies referenced by the assemblies containing the Types which we (recursively) 
    /// need in under to construct that Type under test.</item>
    /// </list>
    /// </summary>
    public class FindInAssembliesReferencedByAssemblyUnderTest : AutoBuildFindTypeRuleAttribute
    {
        static readonly string[] DefaultIgnores = { "mscorlib", "System", "nunit", "Microsoft.VisualStudio", "Moq" };

        /// <summary>
        /// Default value: { "mscorlib", "System", "nunit","Microsoft.VisualStudio", "Moq" }
        /// </summary>
        public string[] IgnoreAssembliesWhereNameStartsWith { get; set; }

        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes = null, object requestingTestFixture = null)
        {
            return requestingTestFixture == null ? null : FindType(type, inOrderToBuildTypes, requestingTestFixture.GetType());
        }

        Type FindType(Type type, IEnumerable<Type> inOrderToBuildTypes, Type requestingType)
        {
            var typesFromWhichToSearch =
                new[] {requestingType}
                    .Union(inOrderToBuildTypes ?? new Type[0]).Where(x => x != null);

            var assembliesToIgnore = (IgnoreAssembliesWhereNameStartsWith ?? new string[0]).Union(DefaultIgnores);

            var assemblyNamesToSearch =
                typesFromWhichToSearch
                    .SelectMany(t => t.Assembly.GetReferencedAssemblies())
                    .Where(a => !assembliesToIgnore.Any(n => a.FullName.StartsWith(n)));

            var allTypesInReferencedAssemblies =
                assemblyNamesToSearch
                    .Select(name => Assembly.Load(name))
                    .SelectMany(a => a.GetTypes());

            var relevantTypes =
                allTypesInReferencedAssemblies.Where(
                    t => !t.IsAbstract
                         && !t.IsInterface
                         && type.IsAssignableFrom(t));

            return relevantTypes.FirstOrDefault();
        }
    }
}