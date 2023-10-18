using System;
using System.Linq;

using R5T.N0000;
using R5T.T0141;
using R5T.T0172.Extensions;


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
            var assemblyFilePaths = Instances.AssemblyFilePathOperator._Platform.Get_AllDependencyAssemblyFilePaths_Inclusive(
                assemblyFilePath.Value);

            var distinctAssemblyFilePaths = Instances.AssemblyFilePathOperator._Platform.Get_DistinctAssemblies(assemblyFilePaths);

            Instances.ConsoleOperator.Output(distinctAssemblyFilePaths);

            Console.WriteLine($"{assemblyFilePaths.Length}: Assembies\n{distinctAssemblyFilePaths.Length}: Distinct Assemblies");
        }

        public void Get_DependencyAssemblyFilePaths_DotnetPack()
        {
            /// Inputs.
            var assemblyFilePath = Instances.FilePaths.Example_Assembly;
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var runtimeVersion = Instances.RuntimeEnvironmentOperator._Platform.Get_CurrentlyExecutingRuntimeVersion();
            var targetFrameworkMoniker = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            var assemblyFilePaths = Instances.AssemblyFilePathOperator._Platform.Get_DependencyAssemblyFilePaths_ForDotnetPack_Inclusive(
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
            var runtimeName = Instances.RuntimeEnvironmentOperator._Platform.Get_CurrentlyExecutingRuntimeName();
            var runtimeVersion = Instances.RuntimeEnvironmentOperator._Platform.Get_CurrentlyExecutingRuntimeVersion();


            /// Run.
            var assemblyFilePaths = Instances.AssemblyFilePathOperator._Platform.Get_DependencyAssemblyFilePaths_ForRuntime_Inclusive(
                assemblyFilePath.Value,
                runtimeName,
                runtimeVersion);

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_RuntimeDirectoryAssemblyFilePaths()
        {
            var assemblyFilePaths = Instances.RuntimeEnvironmentOperator._Platform.Get_CurrentlyExecutingRuntime_AssemblyFilePaths();

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_ExecutableDirectoryAssemblyFilePaths()
        {
            var assemblyFilePaths = Instances.ExecutablePathOperator.Get_AssemblyFilePaths();

            Instances.ConsoleOperator.Output(assemblyFilePaths);
        }

        public void Get_AssemblyDirectoryAssemblyFilePaths()
        {
            var assemblyFilePath =
                //Instances.FilePaths.Example_Assembly
                Instances.AssemblyFilePaths.Executable
                ;

            var assemblyDirectoryAssemblyFilePaths = Instances.AssemblyFilePathOperator.Get_AssemblyDirectoryAssemblyFilePaths(assemblyFilePath);

            foreach (var assemblyDirectoryAssemblyFilePath in assemblyDirectoryAssemblyFilePaths
                .OrderAlphabetically(x => x.Value))
            {
                Console.WriteLine(assemblyDirectoryAssemblyFilePath);
            }
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
            var clrVersion = Instances.RuntimeEnvironmentOperator._Platform.Get_CommonLanguageRuntimeVersion();

            // v4.0.30319:
            Console.WriteLine($"{clrVersion}: Common Language Runtime Version (CLRV)");

            // Compare with the environment version property.
            var dotnetRuntimeVersion = Instances.EnvironmentOperator.Get_DotnetRuntimeVersion();

            // 6.0.21
            Console.WriteLine($"{dotnetRuntimeVersion}: Dotnet Runtime Version (DRV)");
        }

        public void Open_DotnetPackDirectory()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFrameworkMoniker = Instances.TargetFrameworkMonikers.NET_5;


            /// Run.
            //var dotnetPackDirectoryPath = Instances.DotnetPackPathOperator
        }

        public void Get_RuntimeDirectoryPath_ForProjectFile()
        {
            /// Inputs.
            var projectFilePath =
                @"C:\Code\DEV\Git\GitHub\davidcoats\D8S.W0001.Private\source\D8S.W0001\D8S.W0001.csproj".ToProjectFilePath()
                ;


            /// Run.
            var runtimeDirectoryPath = Instances.ProjectFileOperator.Get_RuntimeDirectoryPath_Synchronous(
                projectFilePath,
                out var context);

            Console.WriteLine($"{projectFilePath} =>\n  {runtimeDirectoryPath}\n    {context.ProjectSdkName}: project SDK name\n    {context.TargetFrameworkMoniker}: target framework moniker\n    {context.RuntimeName}: runtime name");
        }

        public void Get_RuntimeName_ForProjectFile()
        {
            /// Inputs.
            var projectFilePath =
                @"C:\Code\DEV\Git\GitHub\davidcoats\D8S.W0001.Private\source\D8S.W0001\D8S.W0001.csproj".ToProjectFilePath()
                ;


            /// Run.
            var runtimeName = Instances.ProjectFileOperator.Get_RuntimeName_Synchronous(projectFilePath);

            Console.WriteLine($"{projectFilePath} =>\n  {runtimeName}: runtime name");
        }

        /// <summary>
        /// Given a project file, get the SDK and target framework from the project.
        /// </summary>
        public void Get_RuntimeTargetInformation_ForProjectFile()
        {
            /// Inputs.
            var projectFilePath =
                @"C:\Code\DEV\Git\GitHub\davidcoats\D8S.W0001.Private\source\D8S.W0001\D8S.W0001.csproj".ToProjectFilePath()
                ;


            /// Run.
            var (sdkName, targetFrameworkMoniker) = Instances.ProjectFileOperator.Get_RuntimeTargetInformation_Synchronous(projectFilePath);

            Console.WriteLine($"{projectFilePath} =>\n  {sdkName}: SDK\n  {targetFrameworkMoniker}: target framework moniker");
        }

        /// <summary>
        /// For a project file, use the publish convention to determine the location of the output assembly file path.
        /// </summary>
        public void Get_ProjectFileOutputAssemblyFilePath_PublishConvention()
        {
            /// Inputs.
            var projectFilePath =
                @"C:\Code\DEV\Git\GitHub\davidcoats\D8S.W0001.Private\source\D8S.W0001\D8S.W0001.csproj".ToProjectFilePath()
                ;


            /// Run.
            var assemblyFilePath = Instances.ProjectFileOperator.Get_OutputAssemblyFilePath_PublishConvention(
                FilePathExists.From(projectFilePath));

            var assemblyFileExists = Instances.FileSystemOperator.Exists_File(assemblyFilePath);

            Console.WriteLine($"{projectFilePath} =>\n  {assemblyFilePath}\n    Exists: {assemblyFileExists}");
        }
    }
}
