#if UNITY_EDITOR
using UnityEditor;

public class Parser
{
    //[MenuItem("Assets/Parser", false, 0)]
    public static void Run()
    {
        AssetDatabase.StartAssetEditing();

        //string assetPath = "Assets{PATH_THEMES}{nameTheme}_{classTheme}.asset";
        //ScriptableClass theme = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath) as ScriptableClass;

        //string[] guids = AssetDatabase.FindAssets(filter, new string[1] { path });
        //prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));

        //GameObject assetObject = null;
        //AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(assetObject)).SetAssetBundleNameAndVariant("asset bundle name", "");
        //EditorUtility.SetDirty(assetObject);

        //ScriptableClass obj = ScriptableObject.CreateInstance<ScriptableClass>();
        //string assetPath = EditorUtility.SaveFilePanelInProject("title", "", "asset", "message", $"path");

        //UnityEngine.Object.DestroyImmediate(obj.GetComponent<SomeClass>());

        //if (!string.IsNullOrEmpty(assetPath))
        //{
        //    AssetDatabase.CreateAsset(obj, assetPath);
        //}

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
#endif