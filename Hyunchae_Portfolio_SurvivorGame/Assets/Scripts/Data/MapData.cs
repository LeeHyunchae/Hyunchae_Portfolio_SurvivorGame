using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

[Serializable]
public class MapData
{
    public int mapWidth;
    public int mapHeight;

    public TileData[] tileDatas;
}
