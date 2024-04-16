using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IngameSceneController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject originPlayerObj;
    [SerializeField] JoystickControlller joystickControlller;
    [SerializeField] FollowCamera followCam;

    private MapCreator mapCreator;
    private PlayerControlller playerControlller;

    private void Awake()
    {
        InitMapCreator();
        InitPlayerControlller();
    }

    private void InitPlayerControlller()
    {
        playerControlller = new PlayerControlller();
        GameObject playerObj = (Instantiate<GameObject>(originPlayerObj));
        playerControlller.Init(playerObj);
        playerControlller.SetJoystick(joystickControlller);

        followCam.SetTarget(playerObj.transform);
    }

    private void InitMapCreator()
    {
        MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");
        mapCreator = new MapCreator();
        mapCreator.Init(tilemap);
        mapCreator.GenerateMap(mapData.mapWidth, mapData.mapHeight);
    }

    
}
