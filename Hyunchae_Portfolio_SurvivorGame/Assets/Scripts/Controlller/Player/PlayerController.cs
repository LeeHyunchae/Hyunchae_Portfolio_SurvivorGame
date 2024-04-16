using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlller
{
    private Transform playerTM;
    private SpriteRenderer playerSprite;

    private Vector2 pos = Vector2.zero;

    public void Init(GameObject _playerObj)
    {
        playerTM = _playerObj.GetComponent<Transform>();
        playerSprite = _playerObj.GetComponent<SpriteRenderer>();

        pos = playerTM.position; ;
    }

    public void SetJoystick(JoystickControlller moveJoystick)
    {
        moveJoystick.OnPointerDownAction = OnMoveJoystickDown;
    }

    public void OnMoveJoystickDown(Vector2 _dir)
    {
        pos += _dir.normalized * 1 * Time.deltaTime;

        playerTM.position = pos;

        playerSprite.flipX = _dir.x < 0;

    }

}
