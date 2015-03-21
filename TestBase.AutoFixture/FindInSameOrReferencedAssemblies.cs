using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// "Referenced assembly" means referenced by any assembly containing any type which we are currently trying to construct.
    /// </summary>
    public class FindInSameOrReferencedAssemblies : AutoFixtureStrategyAttribute
    {
        static readonly string[] DefaultIgnores = { "mscorlib", "System", "nunit", "Microsoft.VisualStudio", "Moq" };

        /// <summary>
        /// Default value: { "mscorlib", "System", "nunit","Microsoft.VisualStudio", "Moq" }
        /// </summary>
        public string[] IgnoreAssembliesWhereNameStartsWith { get; set; }

        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes)
        {
            var assembliesToIgnore = (IgnoreAssembliesWhereNameStartsWith ?? new string[0]).Union(DefaultIgnores);

            var typeInSameAssembly = new FindInSameAssemblyAttribute().FindTypeAssignableTo(type,inOrderToBuildTypes);

            var referencedAssemblyNames = inOrderToBuildTypes.SelectMany(t=>t.Assembly.GetReferencedAssemblies());

            var assemblyNamesToSearch=
                    referencedAssemblyNames
                        .Where(a => !assembliesToIgnore.Any(n => a.FullName.StartsWith(n)));

            var assembliesToSearch = 
                    assemblyNamesToSearch.Select(n => Assembly.LoadFrom(n.Name + ".dll"));

            var types = assembliesToSearch.SelectMany(a => a.GetTypes());

            var foundTypeInReferencedAssembly = 
                    types
                        .FirstOrDefault(
                            t => !t.IsAbstract 
                              && !t.IsInterface 
                              && type.IsAssignableFrom(t));

            return typeInSameAssembly ?? foundTypeInReferencedAssembly;
        }
    }
}