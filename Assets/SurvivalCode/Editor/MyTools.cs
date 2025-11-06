using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
namespace Platformers
{
    public class NewBehaviourScript : MonoBehaviour
    {
       
        // Start is called before the first frame update
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() { 
            Folders.createDefault("_Platformers","Animations","Art","Audio","Fonts","Materials","Models","Prefabs","Scenes","Scripts","Shaders","Textures");
            UnityEditor.AssetDatabase.Refresh();

        }
        static class Folders
        {
            public static void createDefault(string root, params string[] folders)
            {
                var path = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var fullPath = Path.Combine(path, folder);
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                }
            }
        }
    }
}
