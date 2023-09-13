using System;
using System.Linq;

using R5T.T0141;


namespace R5T.S0103
{
    [DemonstrationsMarker]
    public partial interface IDemonstrations : IDemonstrationsMarker
    {
        public void In_ExampleTypesAssemblyContext()
        {
            Instances.ExampleTypesAssemblyOperator.In_AssemblyContext(
                assembly =>
                {
                    var lines = Instances.ReflectionOperations.Get_TypesInAssembly(assembly)
                        .Where(Instances.TypeOperator.Is_Public)
                        .Select(Instances.TypeOperator.Get_NamespacedTypeName)
                        .OrderAlphabetically()
                        ;

                    Instances.ConsoleOperator.Output(lines);
                });
        }

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
            Instances.ReflectionOperator.In_AssemblyContext(
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

        public void Get_DistinctDependencyAssemblyFilePaths()
        {
            /// Inputs.
            var assemblyFilePath = Instances.FilePaths.Example_Assembly;


            /// Run.
            var assemblyFilePaths = Instances.AssemblyFilePathOperator.Get_AllDependencyAssemblyFilePaths_Inclusive(
                assemblyFilePath.Value);

            var distinctAssemblyFilePaths = Instances.AssemblyFilePathOperator.Get_DistinctAssemblies(assemblyFilePaths);

            Instances.ConsoleOperator.Output(distinctAssemblyFilePaths);

            Console.WriteLine($"{assemblyFilePaths.Length}: Assembies\n{distinctAssemblyFilePaths.Length}: Distinct Assemblies");
        }

        public void Get_DependencyAssemblyFilePaths_DotnetPack()
        {
            /// Inputs.
            var assemblyFilePath = Instances.FilePaths.Example_Assembly;
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var runtimeVersion = Instances.RuntimeEnvironmentOperator.Get_CurrentlyExecutingRuntimeVersion();
            var targetFrameworkMoniker = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            var assemblyFilePaths = Instances.AssemblyFilePathOperator.Get_DependencyAssemblyFilePaths_ForDotnetPack_Inclusive(
                assemblyFilePath.Value,
                dotnetPackName.Value,
                runtimeVersion,
                targetFrameworkMoniker.Value);

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_DependencyAssemblyFilePaths()
        {
            /// Inputs.
            var assemblyFilePath = Instances.FilePaths.Example_Assembly;
            var runtimeName = Instances.RuntimeEnvironmentOperator.Get_CurrentlyExecutingRuntimeName();
            var runtimeVersion = Instances.RuntimeEnvironmentOperator.Get_CurrentlyExecutingRuntimeVersion();


            /// Run.
            var assemblyFilePaths = Instances.AssemblyFilePathOperator.Get_DependencyAssemblyFilePaths_ForRuntime_Inclusive(
                assemblyFilePath.Value,
                runtimeName,
                runtimeVersion);

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_RuntimeDirectoryAssemblyFilePaths()
        {
            var assemblyFilePaths = Instances.RuntimeEnvironmentOperator.Get_CurrentlyExecutingRuntime_AssemblyFilePaths();

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_ExecutableDirectoryAssemblyFilePaths()
        {
            var assemblyFilePaths = Instances.ExecutablePathOperator.Get_AssemblyFilePaths();

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_CoreAssemblyFilePath()
        {
            // "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\System.Private.CoreLib.dll"
            var assemblyFilePath = Instances.RuntimeEnvironmentOperator.Get_CurrentlyExecutingRuntime_CoreAssemblyFilePath();

            Console.WriteLine(assemblyFilePath);
        }

        public void Open_DotnetPackDirectoryPath()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            var dotnetPackDirectoryPath = Instances.DotnetPackPathOperator.Open_DotnetPackDirectory(
                dotnetPackName,
                targetFramework);

            Console.WriteLine(dotnetPackDirectoryPath);
        }

        public void Open_RuntimeDirectoryPath()
        {
            // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.21\
            var runtimeDirectoryPath = Instances.RuntimeEnvironmentOperator.Open_RuntimeDirectory();

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
