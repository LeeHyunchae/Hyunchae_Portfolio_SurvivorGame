using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator
{
    private Tilemap tilemap;
    private TileBase tileBase;

    public void Init(Tilemap _tileMap = null)
    {
        tilemap = _tileMap;
        //����� Ÿ�� ������ �ҷ�����
        tileBase = (TileBase)Resources.Load("Tiles/MapTiles");

    }

    public void GenerateMap(int _width,int _height)
    {
        // �ε�� �� �������� ũ�⸦ ���� Ÿ�� �� ��ġ
        tilemap.ClearAllTiles();

        int halfWidth = (int)(_width * 0.5f);
        int halfHeight = (int)(_height * 0.5f);

        for(int i = -halfWidth; i < halfWidth; i++)
        {
            for(int j = -halfHeight; j < halfHeight; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j,0), tileBase);
            }
        }
    }
}
