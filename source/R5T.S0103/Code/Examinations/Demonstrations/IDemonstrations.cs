using System;
using System.Linq;

using R5T.T0141;


namespace R5T.S0103
{
    [DemonstrationsMarker]
    public partial interface IDemonstrations : IDemonstrationsMarker
    {
        public void In_AssemblyContext()
        {
            /// Inputs.
            var assemblyFilePath =
                //@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\coreclr.dll"
                //typeof(string).Assembly.Location
                //@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\System.Private.CoreLib.dll"
                //@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\System.Runtime.dll"
                @"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.21\ref\net6.0\System.Xml.XDocument.dll"
                ;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;

            /// Run.
            Instances.ReflectionOperator.InAssemblyContext(
                assemblyFilePath,
                //assembly =>
                //{
                //    Console.WriteLine(assembly.FullName);
                //}
                //Instances.ReflectionOperations.List_TypesInAssembly_ToConsole
                assembly =>
                {
                    var lines = Instances.ReflectionOperations.Get_TypesInAssembly(assembly)
                        .Where(Instances.TypeOperator.Is_Public)
                        .Select(Instances.TypeOperator.Get_NamespacedTypeName)
                        .OrderAlphabetically()
                        ;

                    Instances.FileOperator.Write_Lines_Synchronous(
                        outputFilePath.Value,
                        lines);
                }
                //assembly =>
                //{
                //    var types = Instances.ReflectionOperations.Get_TypesInAssembly(assembly)
                //        .OrderAlphabetically(Instances.TypeOperator.GetNamespacedTypeName)
                //        .Now();

                //    var firstType = types.First();
                //}
                //assembly =>
                //{
                //    var type = Instances.AssemblyOperator.Select_Type(
                //        assembly,
                //        //"Internal.Console"
                //        //"Internal.Runtime.CompilerServices.Unsafe"
                //        "Internal.Runtime.InteropServices.ComActivator"
                //    //"System.Action`12"
                //    );

                //    //type.IsVisible

                //    Console.WriteLine();
                //}
            );

            Instances.NotepadPlusPlusOperator.Open(outputFilePath);
        }

        public void Get_CoreAssemblyLocation()
        {
            var assemblyFilePath = typeof(string).Assembly.Location;

            // "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\System.Private.CoreLib.dll"
            Console.WriteLine(assemblyFilePath);
        }

        public void Get_RuntimeDirectoryPath()
        {
            var runtimeDirectoryPath = Instances.RuntimeEnvironmentOperator_Internal.Get_RunTimeDirectoryPath();

            // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\
            Console.WriteLine(runtimeDirectoryPath);
        }

        public void Get_RuntimeSystemVersion()
        {
            var clrVersion = Instances.RuntimeEnvironmentOperator.Get_CommonLanguageRuntimeVersion();

            // v4.0.30319:
            Console.WriteLine($"{clrVersion}: Common Language Runtime Version (CLRV)");

            // Compare with the environment version property.
            var dotnetRuntimeVersion = Instances.EnvironmentOperator.Get_DotnetRuntimeVersion();

            // 6.0.21
            Console.WriteLine($"{dotnetRuntimeVersion}: Dotnet Runtime Version (DRV)");
        }
    }
}
