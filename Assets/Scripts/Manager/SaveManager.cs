using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static void Save(StaticPlayerData Player){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log(SceneManager.GetActiveScene().name);

        SaveData saveData = new SaveData(Player);
        Debug.Log(saveData.health);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData Load(){
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }else{
            Debug.Log("Save File Not Found in " + path);
            return null;
        }
    }
}
