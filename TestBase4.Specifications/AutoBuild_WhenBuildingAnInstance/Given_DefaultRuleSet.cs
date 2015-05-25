using System;
using NUnit.Framework;
using TestBase;
using TestBase4.TestCases;
using TestBase4.TestCases.AReferencedAssembly;

namespace TestBase4.Specifications.AutoBuild_WhenBuildingAnInstance
{
    [TestFixture]
    public class Given_DefaultRuleSet
    {
        [TestCase(typeof (ClassWithDefaultConstructor))]
        [TestCase(typeof(ClassWith1ConstructorParam<ClassWithDefaultConstructor>))]
        [TestCase(typeof(ClassWith3ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly>))]
        public void ThenI_BuildInstanceOfRequestedType(Type type)
        {
            var result= AutoBuild.InstanceOf(type);
            //
            Assert.AreEqual(type, result.GetType());
        }


        public void ThenI_BuildInstanceOfRequestedType__AndIGetTheDependenciesRightToo()
        {
            ClassWith3ConstructorParams<INterfaceWithClassInSameAssembly, INterfaceWithFakeInTestAssembly, INterfaceWithClassInNotReferencedAssembly> 
                result = AutoBuild.InstanceOf<ClassWith3ConstructorParams
                                                <INterfaceWithClassInSameAssembly, 
                                                 INterfaceWithFakeInTestAssembly,
                                                 INterfaceWithClassInNotReferencedAssembly>>();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Param1);
            Assert.IsNotNull(result.Param2);
            Assert.IsNotNull(result.Param3);
        }
    }
}
