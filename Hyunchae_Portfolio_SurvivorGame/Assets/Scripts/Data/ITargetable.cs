using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    public bool GetIsDead();
    public Vector2 GetPosition();
    public Transform GetTransform();
    public Bounds GetSpriteBounds();
    public void OnDamaged(DamageData _damageData);
}
