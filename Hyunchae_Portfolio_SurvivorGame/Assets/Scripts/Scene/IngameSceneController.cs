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

    private MapCreator mapCreator;
    private MapData mapData;

    private CharacterManager playerMgr;
    private ItemManager itemManager;

    private PlayerControlller playerController;
    private ItemController itemController;

    private StageController stageController;

    private RectCollisionCalculator rectCollisionCalculator;
    private ITargetable[] targetMonsterArr;
    private ITargetable targetPlayer;

    private void Awake()
    {
        InitMapCreator();

        InitPlayerController();
        InitMonster();
        InitCamera();
        InitItemController();
        InitStageController();

    }

    private void InitPlayerController()
    {
        playerMgr = CharacterManager.getInstance;

        playerController = Instantiate<GameObject>(originPlayerObj).GetComponent<PlayerControlller>();

        playerController.Init();
        playerController.SetJoystick(joystickControlller);

        targetPlayer = playerController;
    }

    private void InitCamera()
    {
        followCam.SetTarget(playerController.GetPlayerTransform);
    }

    private void InitItemController()
    {
        itemManager = ItemManager.getInstance;

        itemController = new ItemController();

        itemController.SetTargetMonsters(targetMonsterArr);
        itemController.Init(playerController.GetPlayerTransform);
    }

    private void InitMapCreator()
    {
        mapData = TableLoader.LoadFromFile<MapData>("Map/MapData");
        mapCreator = new MapCreator();
        mapCreator.Init(tilemap);
        mapCreator.GenerateMap(mapData.mapWidth, mapData.mapHeight);
    }

    private void InitStageController()
    {
        stageController = new StageController();
        stageController.Init(playerController.GetPlayerTransform);
        stageController.SetMapData(mapData);
    }

    private void InitMonster()
    {
        targetMonsterArr = MonsterManager.getInstance.CreateMonsterObjects();

        rectCollisionCalculator = new RectCollisionCalculator();

        rectCollisionCalculator.SetMonsterArr(targetMonsterArr);
        rectCollisionCalculator.SetPlayer(targetPlayer);
    }

    private void Update()
    {
        stageController.Update();
        rectCollisionCalculator.Update();
    }
}
