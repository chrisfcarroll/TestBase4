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
    public class FindInAssembly : AutoFixtureStrategyAttribute
    {
        public FindInAssembly(string assemblyName){
            this.assemblyName = assemblyName;
        }

        static readonly DirectoryInfo BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        readonly string assemblyName;

        public override Type FindTypeAssignableTo(Type type, IEnumerable<Type> inOrderToBuildTypes, Type testFixtureType)
        {
            if(assemblyName.Contains('*') || assemblyName.Contains('?'))
            {
                return FindBestMatchFromAssembliesInBaseDirectory(type); 
            }
            else
            {
                try
                {
                    return Assembly.Load(assemblyName).GetTypes().First(type.IsAssignableFrom);

                }
                catch(Exception)
                {
                    try
                    {
                        return Assembly.LoadFrom(assemblyName).GetTypes().First(type.IsAssignableFrom);
                    }
                    catch(Exception)
                    {
                        return FindBestMatchFromAssembliesInBaseDirectory(type); 
                    }
                }
            }
        }

        Type FindBestMatchFromAssembliesInBaseDirectory(Type type)
        {
            var possibleAssembliesInApplicationBase = BaseDirectory.EnumerateFiles(assemblyName + ".dll").Union(BaseDirectory.EnumerateFiles(assemblyName + ".exe")).OrderByDescending(a => a.FullName.Length);
            //Assembly name can contain wildcards 
            var allTypesInBaseDirectory = possibleAssembliesInApplicationBase.Select(a => 
            {
                try
                {
                    return Assembly.Load(Path.GetFileNameWithoutExtension(a.Name));
                }
                catch
                {
                    return null;
                }
            }).Where(a => a != null).SelectMany(a => a.GetTypes());
            var relevantTypes = allTypesInBaseDirectory.Where(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
            return relevantTypes.FirstOrDefault();
        }
    }
}