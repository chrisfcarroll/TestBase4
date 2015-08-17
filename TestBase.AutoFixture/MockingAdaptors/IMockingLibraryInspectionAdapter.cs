using System;

namespace TestBase
{
    /// <summary>
    /// Optionally implement this together with your <see cref="IMockingLibraryBasicAdapter"/>
    /// </summary>
    public interface IMockingLibraryInspectionAdapter
    {
        /// <summary>
        /// Given an object, which is a mocked object, returns the Mock which owns that object (if the mocking library used supports this functionality).
        /// </summary>
        /// <param name="value">the object, assumed to be a mocked object, of which the owning Mock is wanted</param>
        /// <returns>The Mock which owns <paramref name="value"/>. If <paramref name="value"/> is not recognised then this method should throw.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the mocking library does not support this operation</exception>
        /// <exception cref="ArgumentException">Thrown if the argument is not a mocked object created by this library.</exception>
        /// <remarks>
        /// A mocking library which does not support this functionality should throw an <see cref="InvalidOperationException"/>
        /// </remarks>
        object GetMock(object value);
        /// <param name="value"></param>
        /// <returns><see cref="bool.True"/> if <paramref name="value"/> is a Mocked object created by this library. <see cref="bool.False"/> otherwise.</returns>
        /// <remarks>A mocking library which cannot implement this method should return <see cref="bool.False"/></remarks>
        bool IsThisMyMockObject(object value);
    }
}