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
    [SerializeField] private IngamePanelController ingamePanelController;
    [SerializeField] private HpBarPanelController hpBarPanelController;

    private MapCreator mapCreator;
    private MapData mapData;

    private PlayerController playerController;
    private ItemController itemController;

    private StageController stageController;

    private ITargetable[] targetMonsterArr;
    private ITargetable targetBossMonster;

    private int stageIndex = 0;

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
        playerController = Instantiate<GameObject>(originPlayerObj).GetComponent<PlayerController>();

        playerController.Init();
        playerController.SetJoystick(joystickControlller);

        playerController.SetHPBar(hpBarPanelController.CreateHpBar());
        playerController.SetMapData(mapData);
    }

    private void InitCamera()
    {
        followCam.SetTarget(playerController.GetTransform());
        followCam.SetMapData(mapData);
    }

    private void InitItemController()
    {
        itemController = new ItemController();

        itemController.SetTargetMonsters(targetMonsterArr);
        itemController.Init(playerController.GetTransform());
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
        stageController.Init(playerController);
        stageController.SetMapData(mapData);
        stageController.SetIngamePanel(ingamePanelController);
        stageController.SetStageIndex(stageIndex);
    }

    private void InitMonster()
    {
        MonsterManager monsterManager = MonsterManager.getInstance;
        monsterManager.SetPlayer(playerController);
        targetMonsterArr = monsterManager.CreateMonsterObjects();
        targetBossMonster = monsterManager.CreateBossObject(stageIndex);

        targetMonsterArr[targetMonsterArr.Length -1] = targetBossMonster;

        BossMonsterController boss = targetBossMonster as BossMonsterController;

        boss.SetHPBar(hpBarPanelController.CreateHpBar());

    }

    private void Update()
    {
        stageController.Update();
    }
}
