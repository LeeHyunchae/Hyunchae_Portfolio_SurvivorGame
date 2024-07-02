using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterState
{
    IDLE = 0,
    RUN,
    DEAD
}

public class PlayerController : MonoBehaviour , ITargetable
{
    private string ANIM_CHAR_ID = "CharacterID";
    private string ANIM_CHAR_STATE = "CharacterState";

    private const int RECOVERY_TIME = 1;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector2 pos = Vector2.zero;

    private CharacterManager characterManager;
    private ItemManager itemManager;
    private AugmentManager augmentManager;
    private GlobalData globalData;
    private HpBarController hpBar;
    private float curPlayerHp;
    private Character playerCharacter;
    private Animator animator;

    private bool isDamaged = false;
    private bool isDead = false;
    private float curRecoveryTime = 0;

    public void Init()
    {
        characterManager = CharacterManager.getInstance;
        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;

        itemManager.OnRefreshEquipPassiveList += AddPassiveItem;

        myTransform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        pos = myTransform.position;

        augmentManager = AugmentManager.getInstance;
        augmentManager.onRefreshAgumentActionDict[(int)EAugmentType.PLAYERSTATUS] += OnRefreshChracterAugment;

        InitCharacter();
    }

    private void InitCharacter()
    {
        playerCharacter = characterManager.GetPlayerCharacter;

        animator.SetInteger(ANIM_CHAR_ID, playerCharacter.GetCharacterModel.characterUid);

        spriteRenderer.sprite = characterManager.GetCharacterSprite(playerCharacter.GetCharacterModel.characterUid);
        curPlayerHp = playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus;
    }

    public void SetJoystick(JoystickControlller moveJoystick)
    {
        moveJoystick.OnPointerDownAction = OnDownJoystick;
        moveJoystick.OnPointerUpAction = OnUpJoystick;
    }

    public void SetHPBar(HpBarController _hpBar)
    {
        hpBar = _hpBar;
        hpBar.Init();
        hpBar.SetActive(true);
    }

    private void OnDownJoystick(Vector2 _dir)
    {
        if(isDead)
        {
            return;
        }

        pos += _dir.normalized * playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MOVE_SPEED).multiplierApplyStatus * Time.deltaTime;

        myTransform.position = pos;

        spriteRenderer.flipX = _dir.x < 0;

        animator.SetInteger(ANIM_CHAR_STATE, (int)ECharacterState.RUN);
    }

    private void OnUpJoystick()
    {
        animator.SetInteger(ANIM_CHAR_STATE, (int)ECharacterState.IDLE);
    }

    private void Update()
    { 
        hpBar.UpdatePos(pos);

        if (globalData.GetPause)
        {
            return;
        }

        if(isDamaged)
        {
            RecoveryPlayer();
        }

    }

    private void RecoveryPlayer()
    {
        curRecoveryTime += Time.deltaTime;
        spriteRenderer.color = Color.red;

        if (curRecoveryTime >= RECOVERY_TIME)
        {
            curRecoveryTime = 0;
            isDamaged = false;
            spriteRenderer.color = Color.white;
        }
    }

    private void OnPlayerDie()
    {
        isDead = true;
        animator.SetInteger(ANIM_CHAR_STATE, (int)ECharacterState.DEAD);
        UIManager.getInstance.Show<ResultPanelController>("UI/ResultPanel");
        globalData.SetPause(true);
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
    }

    public void OnDamaged(DamageData _damageData)
    {
        if(isDamaged)
        {
            return;
        }

        //Debug.Log("Player On Damage : " + _damageData.damage);
        //Debug.Log("Player HP : " + curPlayerHp);

        isDamaged = true;

        float damage = _damageData.damage * (1 - (playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_ARMOUR).multiplierApplyStatus / 15));
        curPlayerHp -= damage;

        hpBar.SetHPBarFillAmount(curPlayerHp / playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus);

        if(curPlayerHp <= 0)
        {
            OnPlayerDie();
        }
    }

    public Vector2 GetPosition()
    {
        return myTransform.position;
    }

    public Transform GetTransform()
    {
        return myTransform;
    }

    private void OnRefreshChracterAugment()
    {
        List<AugmentData> augmentDatas = augmentManager.GetCurAugmentList((int)EAugmentType.PLAYERSTATUS);

        int lastIndex = augmentDatas.Count - 1;

        AugmentData augmentData = augmentDatas[lastIndex];

        int firstTypeNum = (int)augmentData.firstAugmentType / 100;

        if(firstTypeNum == (int)EAugmentType.PLAYERSTATUS)
        {
            StatusVariance statusVariance = new StatusVariance()
            {
                characterStatus = (ECharacterStatus)firstTypeNum,
                isRatio = false,
                variance = augmentData.firstAugmentValue
            };

            playerCharacter.UpdateStatusAmount(statusVariance);
        }

        if (augmentData.secondAugmentType != 0)
        {
            int secondTypeNum = (int)augmentData.secondAugmentType / 100;

            if (secondTypeNum == (int)EAugmentType.PLAYERSTATUS)
            {
                StatusVariance statusVariance = new StatusVariance()
                {
                    characterStatus = (ECharacterStatus)secondTypeNum,
                    isRatio = false,
                    variance = augmentData.secondAugmentValue
                };

                playerCharacter.UpdateStatusAmount(statusVariance);
            }
        }
    }

    private void AddPassiveItem()
    {
        List<BaseItemModel> itemList = itemManager.GetAllEquipPassiveItemModelList;

        int slotIndex = itemList.Count - 1;

        PassiveItemModel itemModel = itemList[slotIndex] as PassiveItemModel;

        int varianceCount = itemModel.status_Variances.Count;

        List<ItemStatusVariance> statusVariances = itemModel.status_Variances;

        for (int i = 0; i < varianceCount; i++)
        {
            ItemStatusVariance itemStatusVariance = statusVariances[i];
            int isPlayerStatus = (int)itemStatusVariance.itemStatusType / 100;

            if (isPlayerStatus != (int)EItemStatusTarget.PLAYER)
            {
                continue;
            }

            StatusVariance statusVariance = new StatusVariance()
            {
                characterStatus = (ECharacterStatus)((int)itemStatusVariance.itemStatusType % 100),
                isRatio = itemStatusVariance.isRatio,
                variance = itemStatusVariance.variance
            };

            playerCharacter.UpdateStatusAmount(statusVariance);
        }

    }

    public void StartWave(Vector2 _pos)
    {
        myTransform.position = _pos;
        pos = _pos;

        curPlayerHp = playerCharacter.GetPlayerStatus((int)ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus;
    }
}
