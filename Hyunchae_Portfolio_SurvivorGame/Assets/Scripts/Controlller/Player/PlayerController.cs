using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlller : MonoBehaviour , ITargetable
{
    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector2 pos = Vector2.zero;

    public Transform GetPlayerTransform => myTransform;

    private CharacterManager characterManager;
    private float moveSpeed;

    public void Init()
    {
        characterManager = CharacterManager.getInstance;

        myTransform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        pos = myTransform.position;

        InitCharacter();
    }

    private void InitCharacter()
    {
        Character playerCharacter = characterManager.GetPlayerCharacter;

        spriteRenderer.sprite = characterManager.GetCharacterSprite(playerCharacter.GetCharacterModel.characterUid);
        moveSpeed = playerCharacter.GetPlayerStatus(ECharacterStatus.MOVE_SPEED).baseStatus;
    }

    public void SetJoystick(JoystickControlller moveJoystick)
    {
        moveJoystick.OnPointerDownAction = OnMoveJoystickDown;
    }

    public void OnMoveJoystickDown(Vector2 _dir)
    {
        pos += _dir.normalized * moveSpeed * Time.deltaTime;

        myTransform.position = pos;

        spriteRenderer.flipX = _dir.x < 0;

    }

    public bool GetIsDead()
    {
        return false;
    }

    public Bounds GetSpriteBounds()
    {
        return spriteRenderer.bounds;
    }

    public void OnDamaged()
    {

    }

    public Vector2 GetPosition()
    {
        return myTransform.position;
    }

    public Transform GetTransform()
    {
        return myTransform;
    }
}
