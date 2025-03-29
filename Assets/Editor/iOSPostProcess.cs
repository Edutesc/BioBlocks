
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;

public class iOSPostProcess
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Define o caminho para o arquivo de entitlements na pasta Plugins
            string entitlementsSourcePath = Path.Combine(Application.dataPath, "Plugins/iOS/entitlements.plist");
            
            // Define onde o entitlements será copiado no projeto Xcode
            string entitlementsDestPath = Path.Combine(pathToBuiltProject, "Unity-iPhone/entitlements.plist");
            
            // Copia o arquivo
            if (File.Exists(entitlementsSourcePath))
            {
                File.Copy(entitlementsSourcePath, entitlementsDestPath, true);
                Debug.Log("Entitlements file copied to: " + entitlementsDestPath);
            }
            else
            {
                Debug.LogError("Entitlements file not found at: " + entitlementsSourcePath);
                return;
            }
            
            // Obter o caminho do arquivo .pbxproj
            string pbxprojPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            
            // Carregar o projeto
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxprojPath);
            
            // Obter identificador do target principal
            string targetGuid = pbxProject.GetUnityMainTargetGuid();
            
            // Adicionar o arquivo de entitlements ao projeto
            pbxProject.AddFile("Unity-iPhone/entitlements.plist", "Unity-iPhone/entitlements.plist");
            
            // Configurar o arquivo de entitlements para o target
            pbxProject.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", "Unity-iPhone/entitlements.plist");
            
            // Salvar as alterações no projeto
            pbxProject.WriteToFile(pbxprojPath);
            
            Debug.Log("Entitlements configured in Xcode project!");
        }
    }
}