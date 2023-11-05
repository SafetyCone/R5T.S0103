using System;
using System.Collections.Generic;
using System.Linq;

using R5T.L0053.Extensions;
using R5T.T0132;
using R5T.T0172.Extensions;
using R5T.T0179.Extensions;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface IScripts : IFunctionalityMarker
    {
        /// <summary>
        /// Foreach of my projects, reflect over every member in the output assembly.
        /// Verify that:
        ///     1) identity strings generated directly from the member info (R5T.L0062.F001) match those generated from a signature structure (R5T.L0065.F002),
        ///     2) signature instances round-tripped to a signature string match.
        /// <para>Note: require all publish convention directory file path assemblies to exist (to be already built as part of R5T.S0063.S000).</para>
        /// </summary>
        public void GenerateAndCheck_AllCodebaseMembers()
        {
            /// Inputs.
            var overrideProjectFilePaths = false;
            var overrideProjectFilePaths_Value = @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.T0174\source\R5T.T0174\R5T.T0174.csproj"
                .ToProjectFilePath();
            var overrideIdentityString = false;
            var overrideIdentityString_Value = "M:R5T.T0174.ArrayWrapper`1.op_Implicit(R5T.T0174.ArrayWrapper{`0})~`0[]";
            var writeEqualsResult = false;
            var showSuccesses = false;

            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var outputJsonFilePath = Instances.FilePaths.OutputJsonFilePath;


            /// Run.
            var (humanOutputTextFilePath, logFilePath) = Instances.TextOutputOperator.In_TextOutputContext(
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

                    var results = new List<ProjectFileResult>();

                    foreach (var tuple in projectFilesTuples)
                    {
                        var projectFilePath = tuple.ProjectFilePath;
                        var assemblyFilePath = tuple.AssemblyFilePath.Value;

                        textOutput.WriteInformation(projectFilePath.Value);
                        textOutput.WriteInformation(assemblyFilePath);

                        var result = new ProjectFileResult
                        {
                            ProjectFilePath = projectFilePath,
                        };

                        results.Add(result);

                        // Catch any assembly load exceptions.
                        try
                        {
                            Instances.ProjectFileOperator.In_OutputAssemblyContext_PublishConvention(
                                tuple.ProjectFilePath,
                                (assembly, projectOutputAssemblyContext) =>
                                {
                                    Instances.AssemblyOperator.Foreach_Member(
                                        assembly,
                                        memberInfo =>
                                        {
                                            // Make sure we at least have an identity string with with to refer to the member.
                                            try
                                            {
                                                // Do a real exercise of all member info values (since due to lazy-loading of dependencies, dependencies are not loaded until asked for).
                                                var identityString = Instances.IdentityStringOperator.Get_IdentityString(memberInfo);

                                                // For debugging.
                                                if(overrideIdentityString && identityString.Value != overrideIdentityString_Value)
                                                {
                                                    return;
                                                }

                                                // Do signature parsing.
                                                try
                                                {
                                                    var signature = Instances.SignatureOperator.Get_Signature(memberInfo);

                                                    var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

                                                    var roundTrippedSignature = Instances.SignatureStringOperator.Get_Signature(signatureString);

                                                    var signatureStringPair = new SignatureStringPair
                                                    {
                                                        IdentityString = identityString,
                                                        SignatureString = signatureString,
                                                    };

                                                    var signaturesAreEqual = Instances.SignatureOperator.Are_Equal_ByValue(
                                                        signature,
                                                        roundTrippedSignature);

                                                    if (signaturesAreEqual)
                                                    {
                                                        result.SignatureString_Successes.Add(signatureStringPair);
                                                    }
                                                    else
                                                    {
                                                        if (writeEqualsResult)
                                                        {
                                                            var equalsResult = Instances.SignatureEqualityOperator.Are_Equal(
                                                                signature,
                                                                roundTrippedSignature);

                                                            Instances.ResultOperator.Filter_KeepFailuresOnly(equalsResult);

                                                            Instances.ResultSerializer.Serialize(
                                                                outputJsonFilePath.Value,
                                                                equalsResult);

                                                            Instances.NotepadPlusPlusOperator.Open(outputJsonFilePath);
                                                        }

                                                        result.SignatureString_Failures.Add(signatureStringPair);
                                                    }

                                                    var identityString_FromSignature = Instances.SignatureOperator.Get_IdentityString(signature);

                                                    var identityStringPair = new IdentityStringPair
                                                    {
                                                        IdentityString = identityString,
                                                        SignatureIdentityString = identityString_FromSignature,
                                                    };

                                                    // Make sure to use identity string equals-by-value since identity string types might be unequal.
                                                    var identityStringsAreEqual = identityString.Equals_ByValue(identityString_FromSignature);
                                                    if(identityStringsAreEqual)
                                                    {
                                                        result.IdentityString_Successes.Add(identityStringPair);
                                                    }
                                                    else
                                                    {
                                                        result.IdentityString_Failures.Add(identityStringPair);
                                                    }
                                                }
                                                catch(Exception exception)
                                                {
                                                    textOutput.WriteInformation(exception.Message);

                                                    result.Exceptions.Add(exception);
                                                }
                                            }
                                            catch(Exception exception)
                                            {
                                                textOutput.WriteInformation(exception.Message);

                                                result.Exceptions.Add(exception);
                                            }
                                        });
                                });
                        }
                        catch (Exception exception)
                        {
                            textOutput.WriteInformation(exception.Message);

                            result.Exceptions.Add(exception);
                        }
                    }

                    Instances.Operator.WriteResultsToOutputFile_Synchronous(
                        outputFilePath,
                        results,
                        showSuccesses);
                });

            Instances.NotepadPlusPlusOperator.Open(outputFilePath);
        }

        /// <summary>
        /// Foreach of my projects, reflect over every member in the output assembly.
        /// This provides a ROBUST test of whether all dependency assemblies have been identified or not.
        /// <para>Note: require all publish convention directory file path assemblies to exist (to be already built as part of R5T.S0063.S000).</para>
        /// </summary>
        public void ReflectOver_AllCodebaseOutputAssemblyMembers()
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
            var overrideProjectFilePaths = false;
            var overrideProjectFilePaths_Value = @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.C0003\source\R5T.C0003\R5T.C0003.csproj"
                .ToProjectFilePath();

            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var errorOutputFilePath = Instances.FilePaths.OutputErrorsTextFilePath;


            /// Run.
            var results = new List<string>();

            var (humanOutputTextFilePath, logFilePath) = Instances.TextOutputOperator.In_TextOutputContext(
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
                                        errorOutputFilePath,
                                        lines);

                                    Instances.NotepadPlusPlusOperator.Open(errorOutputFilePath);
                                }
                            });
                    }
                });

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath,
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

                    var assemblyDllFilePathsHash = Instances.FileSystemOperator.Enumerate_DllFilePaths(
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
            var assemblyDllFilePathsHash = Instances.FileSystemOperator.Enumerate_DllFilePaths(
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
                outputFilePath,
                unmatchedAssemblyDllFilePaths.Get_Values()
                    .OrderAlphabetically());

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath2,
                unmatchedDocumentationXmlFilePaths.Get_Values()
                    .OrderAlphabetically());

            Instances.NotepadPlusPlusOperator.Open(
                outputFilePath.Value,
                outputFilePath2.Value);
        }
    }
}
