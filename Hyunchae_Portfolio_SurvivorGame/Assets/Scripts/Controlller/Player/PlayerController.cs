using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour , ITargetable
{
    private const int RECOVERY_TIME = 1;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector2 pos = Vector2.zero;

    private CharacterManager characterManager;
    private ItemManager itemManager;
    private float moveSpeed;
    private AugmentManager augmentManager;
    private HpBarController hpBar;
    private int curPlayerHp;
    private Character playerCharacter;

    private bool isDamaged = false;
    private float curRecoveryTime = 0;

    public void Init()
    {
        characterManager = CharacterManager.getInstance;
        itemManager = ItemManager.getInstance;

        itemManager.OnRefreshEquipPassiveList += AddPassiveItem;

        myTransform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        pos = myTransform.position;

        augmentManager = AugmentManager.getInstance;
        augmentManager.onRefreshAgumentActionDict[(int)EAugmentType.PLAYERSTATUS] += OnRefreshChracterAugment;

        InitCharacter();
    }

    private void InitCharacter()
    {
        playerCharacter = characterManager.GetPlayerCharacter;

        spriteRenderer.sprite = characterManager.GetCharacterSprite(playerCharacter.GetCharacterModel.characterUid);
        moveSpeed = playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MOVE_SPEED).multiplierApplyStatus;
        curPlayerHp = (int)playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus;
    }

    public void SetJoystick(JoystickControlller moveJoystick)
    {
        moveJoystick.OnPointerDownAction = OnMoveJoystickDown;
    }

    public void SetHPBar(HpBarController _hpBar)
    {
        hpBar = _hpBar;
    }

    public void OnMoveJoystickDown(Vector2 _dir)
    {
        pos += _dir.normalized * moveSpeed * Time.deltaTime;

        myTransform.position = pos;

        spriteRenderer.flipX = _dir.x < 0;
    }

    private void Update()
    {
        hpBar.UpdatePos(pos);

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

    public bool GetIsDead()
    {
        return false;
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

        Debug.Log("Player On Damage : " + _damageData.damage);
        Debug.Log("Player HP : " + curPlayerHp);

        isDamaged = true;

        int damage = (int)(_damageData.damage * (1 - (playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_ARMOUR).multiplierApplyStatus / 15)));
        curPlayerHp -= damage;

        hpBar.SetHPBarFillAmount(curPlayerHp / playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus);
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
    }

    private void AddPassiveItem()
    {
        List<BaseItemModel> itemList = itemManager.GetAllEquipPassiveItemModelList;

        int slotIndex = itemList.Count - 1;

        PassiveItemModel itemModel = itemList[slotIndex] as PassiveItemModel;

        int varianceCount = itemModel.status_Variances.Count;

        List<ItemStatusVariance> statusVariances = itemModel.status_Variances;

        for(int i = 0; i <varianceCount; i++)
        {
            ItemStatusVariance itemStatusVariance = statusVariances[i];
            int isPlayerStatus = (int)itemStatusVariance.itemStatusType / 100;

            if(isPlayerStatus != (int)EItemStatusTarget.PLAYER)
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

        //playerCharacter.UpdateStatusAmount()
    }
}
