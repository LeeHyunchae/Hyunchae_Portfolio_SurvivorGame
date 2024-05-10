using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    public Transform GetTransform();
    public void CheckCollision();
}
