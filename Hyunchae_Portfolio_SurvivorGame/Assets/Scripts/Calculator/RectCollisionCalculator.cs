using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectCollisionCalculator
{
    private ITargetable myTargetable;
    private ITargetable playerTargetable;

    public Action OnCollisionAction;

    public void Update()
    {
        CheckRectCollision();
    }

    public void SetMyTargetable(ITargetable _targetable)
    {
        myTargetable = _targetable;

    }

    public void SetPlayerTargetable(ITargetable _player)
    {
        playerTargetable = _player;
    }

    private void CheckRectCollision()
    {
        if (myTargetable.GetIsDead())
        {
            return;
        }

        Rect monsterRect = new Rect(myTargetable.GetPosition(), myTargetable.GetSpriteBounds().size);
        Rect playerRect = new Rect(playerTargetable.GetPosition(), playerTargetable.GetSpriteBounds().size);

        if (playerRect.Overlaps(monsterRect))
        {
            OnCollisionAction?.Invoke();
        }

    }
}
