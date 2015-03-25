using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestBase
{
    /// <summary>
    /// Searches files with a .dll or .exe extension in the <see cref="AppDomain.BaseDirectory"/> directory and
    /// finds Types in them.
    /// </summary>
    public class FindInAssembliesInBaseDirectory : AutoFixtureStrategyAttribute
    {
        static readonly string[] DefaultIgnores = { "System", "nunit", "Moq" };
        static readonly DirectoryInfo BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        /// <summary>
        /// Default value: { "System", "nunit", "Moq" }
        /// </summary>
        public string[] IgnoreAssembliesWhereNameStartsWith { get; set; }

        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes, Type testFixtureType)
        {
            var assembliesToIgnore = (IgnoreAssembliesWhereNameStartsWith ?? new string[0]).Union(DefaultIgnores);

            var possibleAssembliesInApplicationBase =
                    BaseDirectory.EnumerateFiles("*.dll")
                        .Union(
                            BaseDirectory.EnumerateFiles("*.exe"));

            var allTypesInBaseDirectory =
                    possibleAssembliesInApplicationBase
                        .Where(a => !assembliesToIgnore.Any(ia => a.Name.StartsWith(ia)))
                            .Select(a =>
                                    {
                                        try
                                        {
                                            return Assembly.Load(Path.GetFileNameWithoutExtension(a.Name));
                                        }
                                        catch{return null;}
                                    })
                                .Where(a => a!=null)
                                    .SelectMany(a => a.GetTypes());

            var relevantTypes =
                    allTypesInBaseDirectory.Where(
                        t => !t.IsAbstract
                             && !t.IsInterface
                             && type.IsAssignableFrom(t));

            return relevantTypes.FirstOrDefault();
        }
    }
}