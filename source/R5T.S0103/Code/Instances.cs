using System;


namespace R5T.S0103
{
    public static class Instances
    {
        public static F0018.IAssemblyOperator AssemblyOperator => F0018.AssemblyOperator.Instance;
        public static F0000.IEnvironmentOperator EnvironmentOperator => F0000.EnvironmentOperator.Instance;
        public static F0000.IFileOperator FileOperator => F0000.FileOperator.Instance;
        public static Z0015.IFilePaths FilePaths => Z0015.FilePaths.Instance;
        public static F0033.INotepadPlusPlusOperator NotepadPlusPlusOperator => F0033.NotepadPlusPlusOperator.Instance;
        public static F0018.IReflectionOperations ReflectionOperations => F0018.ReflectionOperations.Instance;
        public static F0018.IReflectionOperator ReflectionOperator => F0018.ReflectionOperator.Instance;
        public static IRuntimeEnvironmentOperator RuntimeEnvironmentOperator => S0103.RuntimeEnvironmentOperator.Instance;
        public static Internal.IRuntimeEnvironmentOperator RuntimeEnvironmentOperator_Internal => Internal.RuntimeEnvironmentOperator.Instance;
        public static F0000.ITypeOperator TypeOperator => F0000.TypeOperator.Instance;
        public static F0000.IVersionOperator VersionOperator => F0000.VersionOperator.Instance;
    }
}