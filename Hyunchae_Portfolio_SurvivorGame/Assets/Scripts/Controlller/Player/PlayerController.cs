using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlller
{
    private GameObject playerObj;

    private Transform playerTransform;
    private SpriteRenderer playerSprite;

    private Vector2 pos = Vector2.zero;

    public Transform GetPlayerTransform => playerTransform;

    private CharacterManager characterManager;
    private float moveSpeed;

    public void Init(GameObject _playerObj)
    {
        characterManager = CharacterManager.getInstance;

        playerObj = _playerObj;

        playerTransform = playerObj.GetComponent<Transform>();
        playerSprite = playerObj.GetComponent<SpriteRenderer>();

        pos = playerTransform.position;

        InitCharacter();
    }

    private void InitCharacter()
    {
        Character playerCharacter = characterManager.GetPlayerCharacter;

        playerSprite.sprite = characterManager.GetCharacterSprite(playerCharacter.GetCharacterModel.characterUid);
        moveSpeed = playerCharacter.GetPlayerStatus(ECharacterStatus.MOVE_SPEED).baseStatus;
    }

    public void SetJoystick(JoystickControlller moveJoystick)
    {
        moveJoystick.OnPointerDownAction = OnMoveJoystickDown;
    }

    public void OnMoveJoystickDown(Vector2 _dir)
    {
        pos += _dir.normalized * moveSpeed * Time.deltaTime;

        playerTransform.position = pos;

        playerSprite.flipX = _dir.x < 0;

    }

}
