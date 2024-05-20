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
    [SerializeField] private GameObject tempTarget;

    private MapCreator mapCreator;

    private CharacterManager playerMgr;
    private ItemManager itemManager;

    private PlayerControlller playerController;
    private ItemController itemController;

    private StageController stageController;

    private void Awake()
    {
        InitMapCreator();

        MonsterManager.getInstance.CreateMonsterObjects();

        InitPlayerController();
        InitCamera();
        InitItemController();
        InitStageController();

    }

    private void InitPlayerController()
    {
        playerMgr = CharacterManager.getInstance;

        playerController = new PlayerControlller();

        GameObject playerobj = Instantiate<GameObject>(originPlayerObj);

        playerController.Init(playerobj);
        playerController.SetJoystick(joystickControlller);
    }

    private void InitCamera()
    {
        followCam.SetTarget(playerController.GetPlayerTransform);
    }

    private void InitItemController()
    {
        itemManager = ItemManager.getInstance;

        itemController = new ItemController();

        itemController.Init(playerController.GetPlayerTransform);

    }

    private void InitMapCreator()
    {
        MapData mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");
        mapCreator = new MapCreator();
        mapCreator.Init(tilemap);
        mapCreator.GenerateMap(mapData.mapWidth, mapData.mapHeight);
    }

    private void InitStageController()
    {
        stageController = new StageController();
        stageController.Init(playerController.GetPlayerTransform);
    }

    private void Update()
    {
        itemController.Update();
        stageController.Update();
    }
}
