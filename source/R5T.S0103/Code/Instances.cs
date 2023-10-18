using System;


namespace R5T.S0103
{
    public class Instances :
        L0055.Instances
    {
        public static L0053.IArrayOperator ArrayOperator => L0053.ArrayOperator.Instance;
        public static L0058.IAssemblyFilePathOperator AssemblyFilePathOperator => L0058.AssemblyFilePathOperator.Instance;
        public static L0057.IAssemblyFilePaths AssemblyFilePaths => L0057.AssemblyFilePaths.Instance;
        public static F0018.IAssemblyOperator AssemblyOperator => F0018.AssemblyOperator.Instance;
        public static F0140.IDocumentationXmlFilePathOperator DocumentationXmlFilePathOperator => F0140.DocumentationXmlFilePathOperator.Instance;
        public static T0215.Z000.IDotnetPackNames DotnetPackNames => T0215.Z000.DotnetPackNames.Instance;
        public static L0053.IEnumerableOperator EnumerableOperator => L0053.EnumerableOperator.Instance;
        public static F0000.IEnvironmentOperator EnvironmentOperator => F0000.EnvironmentOperator.Instance;
        public static T0226.L000.IExampleTypesAssemblyOperator ExampleTypesAssemblyOperator => T0226.L000.ExampleTypesAssemblyOperator.Instance;
        public static IFileSystemOperator FileSystemOperator => S0103.FileSystemOperator.Instance;
        public static L0062.F001.IIdentityStringOperator IdentityStringOperator => L0062.F001.IdentityStringOperator.Instance;
        public static L0053.INullOperator NullOperator => L0053.NullOperator.Instance;
        public static L0068.IProjectFileOperator ProjectFileOperator => L0068.ProjectFileOperator.Instance;
        public static F0115.IProjectPathOperator ProjectPathOperator => F0115.ProjectPathOperator.Instance;
        public static F0040.IProjectPathsOperator ProjectPathsOperator => F0040.ProjectPathsOperator.Instance;
        public static F0018.IReflectionOperations ReflectionOperations => F0018.ReflectionOperations.Instance;
        //public static F0018.IReflectionOperator ReflectionOperator => F0018.ReflectionOperator.Instance;
        public static L0057.IReflectionOperator ReflectionOperator => L0057.ReflectionOperator.Instance;
        public static Z0057.ITargetFrameworkMonikers TargetFrameworkMonikers => Z0057.TargetFrameworkMonikers.Instance;
        public static F0000.ITypeOperator TypeOperator => F0000.TypeOperator.Instance;
        public static F0000.IVersionOperator VersionOperator => F0000.VersionOperator.Instance;
    }
}