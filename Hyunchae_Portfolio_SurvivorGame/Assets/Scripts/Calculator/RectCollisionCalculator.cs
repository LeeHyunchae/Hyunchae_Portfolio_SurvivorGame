using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectCollisionCalculator
{
    private ITargetable[] targetMonsterArr;
    private ITargetable targetPlayer;

    private int monsterCapacity;

    public void Update()
    {
        CheckRectCollision();
    }

    public void SetMonsterArr(ITargetable[] _monsterArr)
    {
        targetMonsterArr = _monsterArr;

        monsterCapacity = targetMonsterArr.Length;

    }

    public void SetPlayer(ITargetable _player)
    {
        targetPlayer = _player;
    }

    private void CheckRectCollision()
    {
        int count = monsterCapacity;

        for(int i = 0; i <count; i++)
        {
            ITargetable monster = targetMonsterArr[i];

            if(monster.GetIsDead())
            {
                continue;
            }

            Rect monsterRect = new Rect(monster.GetPosition(), monster.GetSpriteBounds().size);
            Rect playerRect = new Rect(targetPlayer.GetPosition(), targetPlayer.GetSpriteBounds().size);

            if(playerRect.Overlaps(monsterRect))
            {
                Debug.Log("충돌");
            }
        }
    }
}
