using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveWorld(World world)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/world.npc";
        FileStream stream = new FileStream(path, FileMode.Create);

        WorldData data = new WorldData(world);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldData LoadWorld()
    {
        string path = Application.persistentDataPath + "/world.npc";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WorldData data = formatter.Deserialize(stream) as WorldData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
