using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveData 
{
    public static void SaveDeck()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.deck";
        FileStream stream = new FileStream(path, FileMode.Create);


    }
}
