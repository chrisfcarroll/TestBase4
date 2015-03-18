namespace TestBase4.Specifications.AutoFixture.TestTypes
{
    class ClassWith1ConstructorParam<T>
    {
        readonly T param1;
        public ClassWith1ConstructorParam(T param1) { this.param1 = param1; }
    }
}