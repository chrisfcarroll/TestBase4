using System.Collections.Generic;
using NUnit.Framework;
using TestBase;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    [TestFixture]
    class GivenChooseConstructorRule
    {
        [Test]
        public void IChooseFewestParameters_GivenFewestParametersRule()
        {
            IEnumerable<IAutoBuildRule> rules = new[] {new ChooseConstructorWithFewestParametersAttribute()};
            //
            var result = AutoBuild.InstanceByMakingUpParameters<ClassWithMultipleConstructors>(rules);
            //
            Assert.IsNull(result.param1);
            Assert.IsNull(result.param2);
        }
        [Test]
        public void IChooseMostestParameters_GivenMostParametersRule()
        {
            IEnumerable<IAutoBuildRule> rules = new[] { new ChooseConstructorWithMostParametersAttribute() };
            //
            var result = AutoBuild.InstanceByMakingUpParameters<ClassWithMultipleConstructors>(rules);
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
