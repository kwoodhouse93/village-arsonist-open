using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveProgress {
    public static void Save(int latestLevel)
    {
        int loaded = Load();
        if (latestLevel <= loaded)
        {
            return;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, latestLevel);
        stream.Close();
    }

    public static int Load()
    {
        string path = Path();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            int? latestLevel = formatter.Deserialize(stream) as int?;
            stream.Close();
            if (latestLevel == null) {
                return 0;
            }
            return (int)latestLevel;
        }
        return 0;
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/lpbin";
    }
}
