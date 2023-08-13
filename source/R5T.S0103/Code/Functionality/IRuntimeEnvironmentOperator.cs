using System;

using R5T.T0132;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface IRuntimeEnvironmentOperator : IFunctionalityMarker
    {
        /// <summary>
        /// <inheritdoc cref="Internal.IRuntimeEnvironmentOperator.Get_CommonLanguageRuntimeVersion" path="/summary"/>
        /// </summary>
        public Version Get_CommonLanguageRuntimeVersion()
        {
            var versionString = Instances.RuntimeEnvironmentOperator_Internal.Get_CommonLanguageRuntimeVersion();

            var output = Instances.VersionOperator.Parse(versionString);
            return output;
        }
    }
}
