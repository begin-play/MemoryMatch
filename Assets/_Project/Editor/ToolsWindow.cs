using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolsWindow : Editor
{
    [MenuItem("Tools/Go to Persistent Data Path")]
    public static void GoToPersistentDataPath()
    {
        Application.OpenURL("file://" + Application.persistentDataPath);
    }

    [MenuItem("Tools/Clear Player Prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Player Prefs Cleared");
    }

    [MenuItem("Tools/Clear Persistent Data")]
    public static void ClearPersistentData()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Debug.Log("Persistent Data Cleared");
        }
    }
}
