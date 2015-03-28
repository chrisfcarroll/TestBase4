using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestBase4.TestCases.AReferencedAssembly;
using TestBase4.TestCases.ReferencedAssembly2;

namespace TestBase4.TestCases
{
    /// <summary>
    /// "Referenced Assembly" as in <see cref="Assembly.GetReferencedAssemblies"/> --
    /// may not mean what you think it means. Adding an assembly as a reference to a project does <em>not</em> make 
    /// it a referenced assembly. Only if your code actually refers to a Type from that assembly 
    /// does it become referenced.
    /// </summary>
    static class ForceReferencesToReferencedAssemblies
    {
        public static object Force()
        {
            // ReSharper disable once UnusedVariable
            var ref1 = new SomeOtherTypeInReferencedAssembly();
            var ref2 = new SomeOtherTypeInReferencedAssembly2();
            return new object[] { ref1, ref2 };
        }
    }
}
