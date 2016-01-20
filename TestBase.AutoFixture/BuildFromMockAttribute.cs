using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestBase
{
    public class BuildFromMockAttribute : Attribute, IAutoBuildCustomCreateRule
    {
        /// <remarks>
        /// <list type="bullet">
        /// <item>The default mocking library is Moq, used via <see cref="MoqMocker"/>. </item>
        /// <item><strong>Note that your test project still needs a project reference to Moq or your chosen mocking library</strong> in order to create mocks.</item>
        /// </list>
        /// </remarks>
        public IMockingLibraryAdapter MockingLibraryAdapter { get; set; }

        public BuildFromMockAttribute(Type typeToMock, params object[] mockConstructorArgs)
        {
            this.mockConstructorArgs = mockConstructorArgs;
            typesToMock = new [] { typeToMock};
        }

        public BuildFromMockAttribute(params Type[] typesToMock)
        {
            this.typesToMock = typesToMock;
            this.mockConstructorArgs = new object[0];
        }

        /// <summary>
        /// Note that using this overload means that the same list of <paramref name="mockConstructorArgs"/> will be used for all mocked types.
        /// </summary>
        /// <param name="typesToMock"></param>
        /// <param name="mockConstructorArgs"></param>
        public BuildFromMockAttribute(Type[] typesToMock, params object[] mockConstructorArgs)
        {
            this.typesToMock = typesToMock;
            this.mockConstructorArgs = mockConstructorArgs;
        }

        public object CreateInstance(Type type, IEnumerable<Type> typesWaitingToBeBuilt, object originalRequestor)
        {
            if(!typesToMock.Contains(type)) { return null;}
            //
            EnsureMockingLibraryAdapter();
            MockingLibraryAdapter.EnsureMockingAssemblyIsLoadedAndWorkingElseThrow();
            return MockingLibraryAdapter.CreateMockElseNull(type, mockConstructorArgs);
        }

        void EnsureMockingLibraryAdapter() { MockingLibraryAdapter = MockingLibraryAdapter ?? new MoqMocker(); }

        readonly Type[] typesToMock;
        readonly object[] mockConstructorArgs;
    }
}