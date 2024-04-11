using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public Vector3Int tilePos;
    public bool isMove;

    public void SetData(Vector3Int _pos, bool _isMove)
    {
        tilePos = _pos;
        isMove = _isMove;
    }
}
