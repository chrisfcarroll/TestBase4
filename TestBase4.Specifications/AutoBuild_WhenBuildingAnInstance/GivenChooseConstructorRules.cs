using System.Collections.Generic;
using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{


    [TestFixture]
    public class GivenChooseConstructorRule
    {
        [Test]
        public void ThenI_ChooseFewestParameters_GivenFewestParametersRule()
        {
            IEnumerable<IAutoBuildRule> rules = new[] {new ChooseConstructorWithFewestParametersAttribute()};
            //
            var result = AutoBuild.InstanceOf<ClassWithMultipleConstructors>(rules);
            //
            Assert.IsNull(result.param1);
            Assert.IsNull(result.param2);
        }
        [Test]
        public void ThenI_ChooseMostestParameters_GivenMostParametersRule()
        {
            IEnumerable<IAutoBuildRule> rules = new[] { new ChooseConstructorWithMostParametersAttribute() };
            //
            var result = AutoBuild.InstanceOf<ClassWithMultipleConstructors>(rules);
            //
            Assert.IsNotNull(result.param1);
            Assert.IsNotNull(result.param2);
        }
    }

    class ClassWithMultipleConstructors
    {
        public readonly string param1;
        public readonly string param2;

        public ClassWithMultipleConstructors() { }

        public ClassWithMultipleConstructors(string param1) { this.param1 = param1; }

        public ClassWithMultipleConstructors(string param1, string param2) : this(param1)
        {
            this.param2 = param2;
        }
    }
}
