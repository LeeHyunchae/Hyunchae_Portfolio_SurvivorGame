using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCalculaterTest : MonoBehaviour
{
    public GameObject intersectionPointCube;
    public GameObject playerCircle;

    private Vector2 mapLeftTop = new Vector2(-10, 10);
    private Vector2 mapLeftBottom = new Vector2(-10, -10);
    private Vector2 mapRightTop = new Vector2(10, 10);
    private Vector2 mapRightBottom = new Vector2(10, -10);

    public LineRenderer mapLine1, mapLine2, mapLine3, mapLine4, crossLine;

    private SpawnPointCalculater spawnPointCalculater = new SpawnPointCalculater();

    private void Awake()
    {
        mapLine1.startWidth = 1f;
        mapLine1.endWidth = 1f;
        mapLine1.material.color = Color.red;
        mapLine1.positionCount = 2;

        mapLine1.SetPosition(0, mapLeftTop);
        mapLine1.SetPosition(1, mapRightTop);

        mapLine2.startWidth = 1f;
        mapLine2.endWidth = 1f;
        mapLine2.material.color = Color.red;
        mapLine2.positionCount = 2;

        mapLine2.SetPosition(0, mapRightTop);
        mapLine2.SetPosition(1, mapRightBottom);

        mapLine3.startWidth = 1f;
        mapLine3.endWidth = 1f;
        mapLine3.material.color = Color.red;
        mapLine3.positionCount = 2;

        mapLine3.SetPosition(0, mapRightBottom);
        mapLine3.SetPosition(1, mapLeftBottom);

        mapLine4.startWidth = 1f;
        mapLine4.endWidth = 1f;
        mapLine4.material.color = Color.red;
        mapLine4.positionCount = 2;

        mapLine4.SetPosition(0, mapLeftBottom);
        mapLine4.SetPosition(1, mapLeftTop);

        MapData mapData = new MapData();
        mapData.mapWidth = 20;
        mapData.mapHeight = 20;

        spawnPointCalculater.SetMapData(mapData);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(GetRandomPos(out Vector2 _intersectionPos))
            {
                intersectionPointCube.transform.position = _intersectionPos;
            }
            else
            {
                intersectionPointCube.transform.position = new Vector2(10000, 10000);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            float distance = Vector2.Distance(playerCircle.transform.position, intersectionPointCube.transform.position);
            Debug.Log(distance);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerCircle.transform.position = new Vector2(-9, -9);

            if(spawnPointCalculater.GetSpawnPosition(playerCircle.transform.position, out Vector2 _intersectionPos))
            {
                intersectionPointCube.transform.position = _intersectionPos;
            }
            else
            {
                intersectionPointCube.transform.position = new Vector2(10000, 10000);
            }
        }
    }

    private bool GetRandomPos(out Vector2 _intersectionPos)
    {
        Vector2 randomPos = new Vector2(Random.Range(-9, 9), Random.Range(-9, 9));

        playerCircle.transform.position = randomPos;

        return spawnPointCalculater.GetSpawnPosition(randomPos,out _intersectionPos);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(playerCircle.transform.position, 3);
    }
}
