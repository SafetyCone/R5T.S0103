using System;
using System.Threading.Tasks;


namespace R5T.S0103
{
    class Program
    {
        static async Task Main()
        {
            //Scripts.Instance.Find_UnmatchedDotnetPackAssemblyDocumentations();
            //Scripts.Instance.Get_AssembliesWithMatchedDocumentationFiles();
            await Scripts.Instance.ReflectOver_AllCodebaseOutputAssemblyMembers();

            //Demonstrations.Instance.Get_RuntimeSystemVersion();
            //Demonstrations.Instance.Open_RuntimeDirectoryPath();
            //Demonstrations.Instance.Open_DotnetPackDirectoryPath();
            //Demonstrations.Instance.Get_CoreAssemblyLocation();
            //Demonstrations.Instance.Get_ExecutableDirectoryAssemblyFilePaths();
            //Demonstrations.Instance.Get_RuntimeDirectoryAssemblyFilePaths();
            //Demonstrations.Instance.Get_AssemblyDirectoryAssemblyFilePaths();
            //Demonstrations.Instance.Get_DependencyAssemblyFilePaths();
            //Demonstrations.Instance.Get_DependencyAssemblyFilePaths_DotnetPack();
            //Demonstrations.Instance.Get_DistinctDependencyAssemblyFilePaths();
            //Demonstrations.Instance.In_AssemblyContext();
            //Demonstrations.Instance.In_ExampleTypesAssemblyContext();
            //Demonstrations.Instance.Get_ProjectFileOutputAssemblyFilePath_PublishConvention();
            //Demonstrations.Instance.Get_RuntimeTargetInformation_ForProjectFile();
            //Demonstrations.Instance.Get_RuntimeName_ForProjectFile();
            //Demonstrations.Instance.Get_RuntimeDirectoryPath_ForProjectFile();
        }
    }
}