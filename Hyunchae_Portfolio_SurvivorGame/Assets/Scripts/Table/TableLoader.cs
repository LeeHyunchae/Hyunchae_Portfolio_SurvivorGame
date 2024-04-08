using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

public class TableLoader
{
    public async UniTask<T> LoadFromURI<T>(Uri _uri) where T : struct
    {
        //.. TODO :: WEB Request


        var resource = await Resources.LoadAsync(_uri.ToString());

        return JsonConvert.DeserializeObject<T>("");
    }

    public bool SaveFromFile(string _stringData)
    {
        JsonConvert.SerializeObject(_stringData);

        //Todo :: FileSave

        return false;
    }

    public async UniTask<T> LoadFromFile<T>(string _filePath) where T : struct
    {
        var resource = await Resources.LoadAsync(_filePath) as TextAsset;

        return JsonConvert.DeserializeObject<T>(resource.text);
    }
}
