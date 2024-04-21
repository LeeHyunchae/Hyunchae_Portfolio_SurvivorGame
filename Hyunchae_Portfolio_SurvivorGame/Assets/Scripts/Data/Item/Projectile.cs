using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    private Vector2 pos;
    public void Init()
    {
        _transform = this.transform;
        pos = _transform.position;
    }

    public void Fire(int damage)
    {
        
    }

    private void Update()
    {
        pos.x += 5 * -1f * Time.deltaTime;
        _transform.position = pos;
    }
}
