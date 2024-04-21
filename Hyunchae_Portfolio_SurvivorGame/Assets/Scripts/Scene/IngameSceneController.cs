using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IngameSceneController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private JoystickControlller joystickControlller;
    [SerializeField] private FollowCamera followCam;
    [SerializeField] private GameObject originPlayerObj;
    [SerializeField] private GameObject originPlayerWeaponObj;
    [SerializeField] private GameObject tempTarget;

    private MapCreator mapCreator;

    private CharacterManager playerMgr;
    private ItemManager itemManager;

    private PlayerControlller playerController;
    private ItemController itemController;

    private void Awake()
    {
        InitMapCreator();
        InitPlayer();
        InitCamera();
        InitItemController();
    }

    private void InitPlayer()
    {
        playerMgr = CharacterManager.getInstance;

        playerController = new PlayerControlller();
        playerController.Init(Instantiate<GameObject>(originPlayerObj));
        playerController.SetJoystick(joystickControlller);
    }

    private void InitCamera()
    {
        followCam.SetTarget(playerController.GetPlayerTransform);
    }

    private void InitItemController()
    {
        itemManager = ItemManager.getInstance;

        GameObject[] weaponObjArr = new GameObject[ItemManager.WEAPON_CAPACITY];

        for (int i = 0; i < ItemManager.WEAPON_CAPACITY; i++)
        {
            weaponObjArr[i] = Instantiate<GameObject>(originPlayerWeaponObj);
        }

        itemController = new ItemController();
        itemController.SetTarget(playerController.GetPlayerTransform);

        itemController.Init(weaponObjArr);

        itemController.SetTempEnemy(tempTarget.transform);
    }

    private void InitMapCreator()
    {
        MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");
        mapCreator = new MapCreator();
        mapCreator.Init(tilemap);
        mapCreator.GenerateMap(mapData.mapWidth, mapData.mapHeight);
    }

    private void Update()
    {
        itemController.Update();
    }
}
