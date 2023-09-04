using System;


namespace R5T.S0103
{
    public class Instances :
        L0055.Instances
    {
        public static L0058.IAssemblyFilePathOperator AssemblyFilePathOperator => L0058.AssemblyFilePathOperator.Instance;
        public static F0018.IAssemblyOperator AssemblyOperator => F0018.AssemblyOperator.Instance;
        public static F0140.IDocumentationXmlFilePathOperator DocumentationXmlFilePathOperator => F0140.DocumentationXmlFilePathOperator.Instance;
        public static T0215.Z000.IDotnetPackNames DotnetPackNames => T0215.Z000.DotnetPackNames.Instance;
        public static F0138.IDotnetPackPathOperator DotnetPackPathOperator => F0138.DotnetPackPathOperator.Instance;
        public static F0000.IEnvironmentOperator EnvironmentOperator => F0000.EnvironmentOperator.Instance;
        public static F0000.IFileOperator FileOperator => F0000.FileOperator.Instance;
        public static L0057.IFileSystemOperator FileSystemOperator => L0057.FileSystemOperator.Instance;
        public static F0040.IProjectPathsOperator ProjectPathsOperator => F0040.ProjectPathsOperator.Instance;
        public static F0018.IReflectionOperations ReflectionOperations => F0018.ReflectionOperations.Instance;
        public static F0018.IReflectionOperator ReflectionOperator => F0018.ReflectionOperator.Instance;
        public static IRuntimeEnvironmentOperator RuntimeEnvironmentOperator => S0103.RuntimeEnvironmentOperator.Instance;
        public static Internal.IRuntimeEnvironmentOperator RuntimeEnvironmentOperator_Internal => Internal.RuntimeEnvironmentOperator.Instance;
        public static Z0057.ITargetFrameworkMonikers TargetFrameworkMonikers => Z0057.TargetFrameworkMonikers.Instance;
        public static F0000.ITypeOperator TypeOperator => F0000.TypeOperator.Instance;
        public static F0000.IVersionOperator VersionOperator => F0000.VersionOperator.Instance;
    }
}