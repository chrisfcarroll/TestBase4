using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoFixture
{
    [TestFixture]
    public class WhenYou_RunTestSetUpInAFixtureInheritingFromTestBaseForT : TestBaseFor<ClassWithDefaultConstructor>
    {
        [Test]
        public void ThenI_SetUnitUnderTestToAnInstanceOfT()
        {
            Assert.IsNotNull(UnitUnderTest);
            Assert.That(UnitUnderTest, Is.AssignableTo<ClassWithDefaultConstructor>());
        }
    }

    public class ClassWithDefaultConstructor { }

}
