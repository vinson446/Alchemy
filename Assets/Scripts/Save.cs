using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save
{
    public static void SaveGame(GameManager gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "game.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/game.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
