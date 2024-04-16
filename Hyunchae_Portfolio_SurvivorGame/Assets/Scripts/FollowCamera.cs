using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Vector2 mapSize;

    private float cameraMoveSpeed = 1;
    private float height;
    private float width;

    private Transform _transform;
    private Vector3 pos;

    private void Start()
    {
        height = mapSize.y * 0.9f;
        width = mapSize.x * 0.75f;

        _transform = this.transform;
        pos = _transform.position;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        LimitCameraArea();
    }

    private void LimitCameraArea()
    {
        pos = Vector3.Lerp(pos, target.position, Time.deltaTime * cameraMoveSpeed);

        pos.z = -10;
        //float lx = mapSize.x - width;
        //float clampX = Mathf.Clamp(pos.x, -lx + center.x, lx + center.x);

        //float ly = mapSize.y - height;
        //float clampY = Mathf.Clamp(pos.y, -ly + center.y, ly + center.y);

        //pos.x = clampX;
        //pos.y = clampY;

        _transform.position = pos;
    }

}
