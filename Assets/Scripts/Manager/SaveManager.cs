using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static void Save(PlayerController Player, string scene=null){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(Player, scene);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData Load(PlayerController Player){
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }else{
            PlayerData data = new PlayerData(Player);
            data.Reset();
            return data;
        }
    }

    public static void DeleteSaveFile(){
        string path = Application.persistentDataPath + "/LittleDungeon.save";
        if(File.Exists(path)){
            File.Delete(path);
        }
    }
}
