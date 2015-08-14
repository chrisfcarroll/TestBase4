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
    /// In a typical edit/test from the IDE usage, the BaseDirectory will the the Test Projects bin\Debug directory.
    /// </summary>
    public class FindInAssembliesInBaseDirectoryAttribute : AutoBuildFindTypeRuleAttribute
    {
        static readonly string[] DefaultIgnores = { "System", "nunit", "Moq" };
        static readonly DirectoryInfo BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        /// <summary>
        /// Default value: { "System", "nunit", "Moq" }
        /// </summary>
        public string[] IgnoreAssembliesWhereNameStartsWith { get; set; }

        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null) {
            return FindTypeAssignableTo(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
        public override Type FindTypeAssignableTo(string typeName, IEnumerable<Type> theStackOfTypesToBuild = null, object requestedBy = null)
        {
            return FindTypeAssignableTo(t => !t.IsAbstract && !t.IsInterface && t.FullName.EndsWith(typeName));
        }


        Type FindTypeAssignableTo(Func<Type, bool> filterBy)
        {
            var possibleAssembliesInApplicationBase =
                    BaseDirectory.EnumerateFiles("*.dll")
                        .Union(BaseDirectory.EnumerateFiles("*.exe"));

            var assembliesToIgnore = (IgnoreAssembliesWhereNameStartsWith ?? new string[0]).Union(DefaultIgnores);

            var allTypesInBaseDirectory =
                possibleAssembliesInApplicationBase
                    .Where(a => !assembliesToIgnore.Any(ia => a.Name.StartsWith(ia)))
                    .Select(a =>
                            {
                                try { return Assembly.Load(Path.GetFileNameWithoutExtension(a.Name)); } catch {
                                    return null;
                                }
                            })
                    .Where(a => a != null)
                    .SelectMany(a => a.GetTypes());

            return allTypesInBaseDirectory.Where(filterBy).FirstOrDefault();
        }
    }
}