using System;
using System.Linq;

using R5T.T0132;
using R5T.T0172.Extensions;
using R5T.T0179.Extensions;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface IScripts : IFunctionalityMarker
    {
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

            Instances.WindowsExplorerOperator.OpenDirectoryInExplorer(
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
