using System;
using System.Reflection;

using R5T.T0141;


namespace R5T.S0103
{
    [ExperimentsMarker]
    public partial interface IExperiments : IExperimentsMarker
    {
        /// <summary>
        /// When iterating over types in an assembly, are the nested types of a type included in the list of types in the assembly?
        /// Note: this documentation (<see href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.definedtypes?view=net-7.0"/>)
        /// suggests that nested types ARE returned by <see cref="Assembly.DefinedTypes"/>.
        /// </summary>
        public void NestedTypesAreTypesInAssembly()
        {

        }
    }
}
