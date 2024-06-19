using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour , ITargetable
{
    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector2 pos = Vector2.zero;

    private CharacterManager characterManager;
    private float moveSpeed;
    private AugmentManager augmentManager;
    private HpBarController hpBar;
    private int playerCurHp;
    private Character playerCharacter;

    public void Init()
    {
        characterManager = CharacterManager.getInstance;

        myTransform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        pos = myTransform.position;

        augmentManager = AugmentManager.getInstance;
        augmentManager.onRefreshAgumentActionDict[(int)EAugmentType.PLAYERSTATUS] += OnRefreshChraterAugment;

        InitCharacter();
    }

    private void InitCharacter()
    {
        playerCharacter = characterManager.GetPlayerCharacter;

        spriteRenderer.sprite = characterManager.GetCharacterSprite(playerCharacter.GetCharacterModel.characterUid);
        moveSpeed = playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MOVE_SPEED).multiplierApplyStatus;
        playerCurHp = (int)playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus;
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
    }

    public bool GetIsDead()
    {
        return false;
    }

    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
    }

    public void OnDamaged(int _damage)
    {
        int damage = (int)(_damage * (1 - (playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_ARMOUR).multiplierApplyStatus / 15)));
        playerCurHp -= damage;

        hpBar.SetHPBarFillAmount(playerCharacter.GetPlayerStatus(ECharacterStatus.PLAYER_MAXHP).multiplierApplyStatus / playerCurHp);
    }

    public Vector2 GetPosition()
    {
        return myTransform.position;
    }

    public Transform GetTransform()
    {
        return myTransform;
    }

    private void OnRefreshChraterAugment()
    {
        List<AugmentData> augmentDatas = augmentManager.GetCurAugmentList((int)EAugmentType.PLAYERSTATUS);
    }
}
