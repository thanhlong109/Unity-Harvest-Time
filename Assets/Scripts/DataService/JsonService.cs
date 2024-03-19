
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
public class JsonService : IDataService
{
    public T LoadData<T>(string relativePath, bool encrypted)
    {
        throw new System.NotImplementedException();
    }

    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.dataPath + relativePath;
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("data exist!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("create file for first time");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
            Debug.Log("save at:"+path);

            return true;

        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }



    }
}
