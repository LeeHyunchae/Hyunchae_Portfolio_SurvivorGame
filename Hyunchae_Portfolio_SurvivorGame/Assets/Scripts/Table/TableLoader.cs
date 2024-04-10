using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public class TableLoader
{
    private readonly string APP_PATH = Application.dataPath;    
    private readonly string JSON_DATA_PATH = "JsonData";

    public void SaveToJson(string _directory,object _data,string _fileName)
    {
        string json = JsonConvert.SerializeObject(_data);

        string path = Path.Combine(APP_PATH, JSON_DATA_PATH ,_directory,_fileName); // File Name?

        File.WriteAllText(path, json);

    }

    public object LoadFromFile(string _filePath)
    {
        string path = Path.Combine(APP_PATH, JSON_DATA_PATH, _filePath);

        var json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject(json);
    }
}
