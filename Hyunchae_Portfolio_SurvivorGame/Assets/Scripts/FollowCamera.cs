using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private const float HEIGHTCORRECT = 0.15f;
    private const float WIDTHCORRECT = 0.3f;

    private Transform target;

    private float cameraMoveSpeed = 2;
    private float height;
    private float width;

    private Transform _transform;
    private Vector3 pos;
    private MapData mapData;

    private void Start()
    {
        _transform = this.transform;
        pos = _transform.position;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;

        height = mapData.mapHeight * HEIGHTCORRECT;
        width = mapData.mapWidth * WIDTHCORRECT;
    }

    private void FixedUpdate()
    {
        LimitCameraArea();
    }

    private void LimitCameraArea()
    {
        pos = Vector3.Lerp(pos, target.position, Time.deltaTime * cameraMoveSpeed);

        pos.z = -10;
        
        if(Mathf.Abs(pos.y) >= height)
        {
            pos.y = _transform.position.y;
        }

        if(Mathf.Abs(pos.x) >= width)
        {
            pos.x = _transform.position.x;
        }

        _transform.position = pos;
    }

}
