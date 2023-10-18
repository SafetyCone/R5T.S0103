using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.L0053.Extensions;
using R5T.T0132;
using R5T.T0172.Extensions;
using R5T.T0179.Extensions;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface IScripts : IFunctionalityMarker
    {
        public async Task ReflectOver_AllCodebaseOutputAssemblyMembers()
        {
            // For a project file path:
            //  * Determine the publish convention's output assembly file path. (Requires reading the project file.)
            //  * Determine the dotnet runtime(not pack) to use for the project (Blazor and ASP.NET projects are different).
            //      * Get the runtime name for the project.
            //          * Determine the SDK of the project. (Required for runtime selection.)
            //          * Map SDK name to runtime name.
            //  * Determine the target framework of the project. (Required for runtime selection.)
            //  * Get the dotnet pack directory for a project.
            //  * Get the runtime directory for a project.
            //  * Get all dependency assembly file paths for the assembly (assembly directory + runtime directory).

            /// Inputs.
            var overrideProjectFilePaths = true;
            var overrideProjectFilePaths_Value = @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.C0003\source\R5T.C0003\R5T.C0003.csproj"
                .ToProjectFilePath();

            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var errorOutputFilePath = Instances.FilePaths.OutputErrorsTextFilePath;


            /// Run.
            var results = new List<string>();

            var (humanOutputTextFilePath, logFilePath) = await Instances.TextOutputOperator.In_TextOutputContext(
                nameof(ReflectOver_AllCodebaseOutputAssemblyMembers),
                textOutput =>
                {
                    var projectFilePaths = overrideProjectFilePaths
                        ? Instances.EnumerableOperator.From(overrideProjectFilePaths_Value)
                        : Instances.FileSystemOperator.Find_AllProjectFilePaths(
                            textOutput)
                            .Select(projectFilePath => projectFilePath.ToProjectFilePath())
                        ;

                    var projectFilesTuples = Instances.ProjectPathOperator.CreateProjectFilesTuples(
                        projectFilePaths,
                        textOutput)
                        .Where(tuple => true
                            && Instances.NullOperator.Is_NotNull(tuple.AssemblyFilePath)
                            && Instances.FileSystemOperator._Platform.Exists_File(
                                tuple.AssemblyFilePath.Value)
                        )
                        .Now();

                    foreach (var tuple in projectFilesTuples)
                    {
                        var assemblyFilePath = tuple.AssemblyFilePath.Value;

                        textOutput.WriteInformation(tuple.ProjectFilePath.Value);
                        textOutput.WriteInformation(assemblyFilePath);

                        Instances.ProjectFileOperator.In_OutputAssemblyContext_PublishConvention(
                            tuple.ProjectFilePath,
                            (assembly, projectOutputAssemblyContext) =>
                            {
                                try
                                {
                                    Instances.AssemblyOperator.Foreach_Member(
                                        assembly,
                                        memberInfo =>
                                        {
                                            // This is too easy.
                                            //namesList.Add(memberInfo.Name);

                                            // Do a real exercise of all member info values (since due to lazy-loading of dependencies, dependencies are not loaded until asked for).
                                            var identityString = Instances.IdentityStringOperator.Get_IdentityString(memberInfo);

                                            results.Add(identityString.Value);
                                        });
                                }
                                catch(Exception exception)
                                {
                                    var lines = Instances.EnumerableOperator.From(projectOutputAssemblyContext.ProjectFilePath.Value)
                                        .Append(projectOutputAssemblyContext.ProjectSdkName.Value)
                                        .Append(projectOutputAssemblyContext.RuntimeName.Value)
                                        .Append(projectOutputAssemblyContext.TargetFrameworkMoniker.Value)
                                        .Append("")
                                        .Append(exception.Message)
                                        .Append("")
                                        .Append(projectOutputAssemblyContext.DependencyAssemblyFilePaths.Get_Values());

                                    Instances.FileOperator.Write_Lines_Synchronous(
                                        errorOutputFilePath.Value,
                                        lines);

                                    Instances.NotepadPlusPlusOperator.Open(errorOutputFilePath);
                                }
                            });

                        //await Instances.ReflectionOperator.In_AssemblyContext(
                        //    assemblyFilePath,
                        //    async assembly =>
                        //    {
                        //        // Foreach member element in the assembly (event, field, method, namespace, property, type), get the identity name.
                        //        await Instances.AssemblyOperator.Foreach_Member(
                        //            assembly,
                        //            memberInfo =>
                        //            {
                        //                //// This is too easy.
                        //                //namesList.Add(memberInfo.Name);

                        //                // Do a real exercise of all member info values (since due to lazy-loading of dependencies, dependencies are not loaded until asked for).
                        //                var identityString = Instances.IdentityStringOperator.Get_IdentityString(memberInfo);

                        //                results.Add(identityString.Value);

                        //                return Task.CompletedTask;
                        //            });
                        //    });
                    }

                    return Task.CompletedTask;
                });

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath.Value,
                results);

            Instances.NotepadPlusPlusOperator.Open(outputFilePath);
        }

        /// <summary>
        /// Given a .NET pack name and target framework, get all assembly files in .NET pack directory, then find all paired XML documentation files.
        /// Output both the paired and unpaired assembly files (and their documentation files).
        /// </summary>
        public void Get_AssembliesWithMatchedDocumentationFiles()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var outputFilePath2 = Instances.FilePaths.OutputTextFilePath_Secondary;

            /// Run.
            var (humanOutputTextFilePath, logFilePath) = Instances.TextOutputOperator.In_TextOutputContext_Synchronous(
                nameof(Get_AssembliesWithMatchedDocumentationFiles),
                textOutput =>
                {
                    var dotnetPackDirectoryPath = Instances.DotnetPackPathOperator.Get_DotnetPackDirectoryPath(
                        dotnetPackName,
                        targetFramework);

                    textOutput.WriteInformation($"Dotnet pack directory path ({dotnetPackName}:{targetFramework}):\n\t{dotnetPackDirectoryPath}");

                    var assemblyDllFilePathsHash = Instances.FileSystemOperator.Enumerate_DllFiles(
                        dotnetPackDirectoryPath)
                        .ToHashSet();

                    // Get all documentation files (assuming all XML files are documentation files).
                    var pairedFilePaths = Instances.AssemblyFilePathOperator.Get_PairedAssemblyXmlDocumentationFilePaths(
                        dotnetPackDirectoryPath);

                    var lines = pairedFilePaths.PairedFilePaths
                        .OrderAlphabetically(x => x.Key.Value)
                        .Select(x => $"{x.Key}\n\t{x.Value}")
                        ;

                    Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                        outputFilePath.Value,
                        lines);

                    Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                        outputFilePath2.Value,
                        pairedFilePaths.UnpairedAssemblyFilePaths
                            .Get_Values()
                            .OrderAlphabetically());
                });

            Instances.NotepadPlusPlusOperator.Open(
                humanOutputTextFilePath);
        }

        /// <summary>
        /// Given a .NET pack name and target framework, discover all assembly file paths in the .NET pack directory,
        /// then discover all documentation files,
        /// then match documentation and assembly files.
        /// Output the file documentation files that are matched, and those that are unmatched.
        /// </summary>
        public void Find_UnmatchedDotnetPackAssemblyDocumentations()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var outputFilePath2 = Instances.FilePaths.OutputTextFilePath_Secondary;


            /// Run.
            // Get the dotnet pack directory path.
            var dotnetPackDirectoryPath = Instances.DotnetPackPathOperator.Get_DotnetPackDirectoryPath(
                dotnetPackName,
                targetFramework);

            Instances.WindowsExplorerOperator._Platform.Open(
                dotnetPackDirectoryPath.Value);

            // Get all assembly DLL files.
            var assemblyDllFilePathsHash = Instances.FileSystemOperator.Enumerate_DllFiles(
                dotnetPackDirectoryPath)
                .ToHashSet();

            // Get all documentation files (assuming all XML files are documentation files).
            var documentationXmlFilePathsHash = Instances.DocumentationXmlFilePathOperator.Get_DocumentationXmlFilePaths_AssumeAllXmls(
                dotnetPackDirectoryPath)
                .ToHashSet();

            // Match.
            var documentationXmlFilePathsByAssemblyFilePath = assemblyDllFilePathsHash
                .ToDictionary(
                    x => x,
                    x => Instances.ProjectPathsOperator.GetDocumentationFilePath_ForAssemblyFilePath(
                        x.Value)
                        .ToDocumentationXmlFilePath());

            var unmatchedAssemblyDllFilePaths = assemblyDllFilePathsHash
                .Where(x => !documentationXmlFilePathsHash.Contains(
                    documentationXmlFilePathsByAssemblyFilePath[x]))
                .Now();

            var unmatchedDocumentationXmlFilePaths = documentationXmlFilePathsHash.Except(
                documentationXmlFilePathsByAssemblyFilePath.Values)
                .Now();

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath.Value,
                unmatchedAssemblyDllFilePaths.Get_Values()
                    .OrderAlphabetically());

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath2.Value,
                unmatchedDocumentationXmlFilePaths.Get_Values()
                    .OrderAlphabetically());

            Instances.NotepadPlusPlusOperator.Open(
                outputFilePath.Value,
                outputFilePath2.Value);
        }
    }
}
