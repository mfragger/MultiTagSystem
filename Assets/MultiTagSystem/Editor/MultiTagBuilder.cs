using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace MultiTagSystem
{
    public class MultiTagBuilder : EditorWindow
    {
        private const string Location = "MultiTagSystem";
        private const string ClassName = "Tags";

        [MenuItem("MultiTag/ShowWindow")]
        private static void ShowWindow()
        {
            GetWindow<MultiTagBuilder>("MultiTagBuilder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        }

        [MenuItem("MultiTag/Rebuild Tags")]
        [ExecuteInEditMode]
        private static void BuildTags()
        {
            string folderPath = Application.dataPath + "/" + Location;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText($"{folderPath}/{ClassName}.cs", BuildClass(ClassName, UnityEditorInternal.InternalEditorUtility.tags));
            AssetDatabase.ImportAsset($"Assets/{Location}/{ClassName}.cs", ImportAssetOptions.ForceUpdate);
        }

        private static string BuildClass(string className, string[] internalTagsArray)
        {
            string arrayOutput = "";
            string output = "//This is auto generated DO NOT MODIFY or REMOVE\r\nusing System.Collections.Generic;\r\n\r\nnamespace MultiTagSystem\r\n{\r\n\tpublic static class " + className + "\r\n\t{\r\n";
            for (int i = 0; i < internalTagsArray.Length; i++)
            {
                var tag = internalTagsArray[i];
                var consTag = RemoveSpaces(tag);
                output += "\t\tpublic const string " + consTag + " = \"" + tag + "\";\r\n";

                arrayOutput += "\r\n\t\t\t" + consTag;
                if (i < internalTagsArray.Length - 1)
                {
                    arrayOutput += ",";
                }
            }

            output += "\r\n\t\tpublic static readonly HashSet<string> Set = new HashSet<string>\r\n\t\t{" + arrayOutput + "\r\n\t\t};\r\n\t}\r\n}";
            return output;
        }

        private static string RemoveSpaces(string v)
        {
            return v.Replace(" ", "_");
        }
    }
}