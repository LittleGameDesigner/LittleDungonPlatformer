using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static void Save(){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log(SceneManager.GetActiveScene().name);

        formatter.Serialize(stream, SceneManager.GetActiveScene().name);
        stream.Close();
    }

    public static string Load(){
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            string sceneName = formatter.Deserialize(stream) as string;
            stream.Close();

            return sceneName;
        }else{
            Debug.Log("Save File Not Found in " + path);
            return null;
        }
    }
}
