using System;
using System.Collections.Generic;
using System.Linq;

using R5T.T0132;
using R5T.T0181;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface IOperator : IFunctionalityMarker
    {
        public void WriteResultsToOutputFile_Synchronous(
            ITextFilePath textFilePath,
            IList<ProjectFileResult> results,
            bool showSuccesses)
        {
            var resultsToOutput = results
                .Where(result =>
                {
                    // Note: will exclude project that have no members (which do exist)
                    var output = false
                        || result.Exceptions.Any()
                        || result.IdentityString_Failures.Any()
                        || result.SignatureString_Failures.Any()
                        || (showSuccesses
                            && (false
                                || result.IdentityString_Successes.Any()
                                || result.SignatureString_Successes.Any()));

                    return output;
                })
                .Now();

            var lines = Instances.EnumerableOperator.From("Signature strings and identity strings for all members in output assemblies of project files.")
                .AppendIf(resultsToOutput.None(), "SUCCESS: All signature strings and identity strings matched.")
                .AppendIf(resultsToOutput.Any(), resultsToOutput
                    .SelectMany(result =>
                    {
                        var output = Instances.EnumerableOperator.From($"{result.ProjectFilePath}:")
                            .AppendIf(result.Exceptions.None(), "<No exceptions>")
                            .AppendIf(result.Exceptions.Any(), Instances.EnumerableOperator.From($"{result.Exceptions.Count} - Exceptions:")
                                .Append(result.Exceptions
                                    .Select(exception => $"{exception.Message}\n")
                                )
                            )
                            .AppendIf(result.IdentityString_Failures.None(), "<No identity string mismatches>")
                            .AppendIf(result.IdentityString_Failures.Any(),
                                Instances.EnumerableOperator.From($"{result.IdentityString_Failures.Count} - Identity string mismatches (identity string/signature identity string):")
                                .Append(result.IdentityString_Failures
                                    .Select(pair => $"\t{pair.IdentityString}\n\t{pair.SignatureIdentityString}\n")
                                )
                            )
                            .AppendIf(showSuccesses && result.IdentityString_Successes.None(), "<No identity string successes>")
                            .AppendIf(showSuccesses && result.IdentityString_Successes.Any(),
                                Instances.EnumerableOperator.From($"{result.IdentityString_Successes.Count} - Identity string successes:")
                                .Append(result.IdentityString_Successes
                                    .Select(pair => $"\t{pair.IdentityString.Value}")
                                )
                            )
                            .AppendIf(result.SignatureString_Failures.None(), "<No signature string mismatches>")
                            .AppendIf(result.SignatureString_Failures.Any(),
                                Instances.EnumerableOperator.From($"{result.SignatureString_Failures.Count} - Signature string roundtrip failures (identity string/signature string):")
                                .Append(result.SignatureString_Failures
                                    .Select(pair => $"\t{pair.IdentityString}\n\t{pair.SignatureString}\n")
                                )
                            )
                            .AppendIf(showSuccesses && result.SignatureString_Successes.None(), "<No signature string successes>")
                            .AppendIf(showSuccesses && result.SignatureString_Successes.Any(),
                                Instances.EnumerableOperator.From($"{result.SignatureString_Successes.Count} - Signature string successes (identity string/signature string):")
                                .Append(result.SignatureString_Successes
                                    .Select(pair => $"\t{pair.IdentityString}\n\t{pair.SignatureString}\n")
                                )
                            )
                            ;

                        return output;
                    })
                );

            Instances.FileOperator.Write_Lines_Synchronous(
                textFilePath,
                lines);
        }
    }
}
