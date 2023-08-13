using System;
using System.Runtime.InteropServices;

using R5T.T0132;


namespace R5T.S0103.Internal
{
    [FunctionalityMarker]
    public partial interface IRuntimeEnvironmentOperator : IFunctionalityMarker
    {
        /// <summary>
        /// <inheritdoc cref="RuntimeEnvironment.GetRuntimeDirectory" path="/summary"/>
        /// <para>Example: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\</para>
        /// </summary>
        public string Get_RunTimeDirectoryPath()
        {
            var output = RuntimeEnvironment.GetRuntimeDirectory();
            return output;
        }

        /// <summary>
        /// <inheritdoc cref="RuntimeEnvironment.GetSystemVersion" path="/summary"/>
        /// <para>Example: v4.0.30319</para>
        /// </summary>
        public string Get_CommonLanguageRuntimeVersion()
        {
            var output = RuntimeEnvironment.GetSystemVersion();
            return output;
        }
    }
}
