using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationOfLineTest : MonoBehaviour
{
    public GameObject intersectionPointCube;
    public GameObject playerCircle;

    private Vector2 mapLeftTop = new Vector2(-10, 10);
    private Vector2 mapLeftBottom = new Vector2(-10, -10);
    private Vector2 mapRightTop = new Vector2(10, 10);
    private Vector2 mapRightBottom = new Vector2(10, -10);

    private Vector2 randomLineEndPos;

    private const int CREATE_LIMIT_RANGE = 3;

    public LineRenderer ranLine, mapLine1,mapLine2,mapLine3,mapLine4,crossLine;

    private bool isRight;
    private bool isTop;
    private Vector2 intersectionPosition = Vector2.zero;
    private bool isTooClose = false;

    private void Awake()
    {
        ranLine.startWidth = 1f;
        ranLine.endWidth = 1f;
        ranLine.material.color = Color.blue;
        ranLine.positionCount = 2;

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


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRandomDirection();
        }

        if(isRight)
        {
            if (CrossCheck2D(mapRightTop, mapRightBottom, playerCircle.transform.position, randomLineEndPos))
            {
                intersectionPosition = GetCrossPosition(mapRightTop, mapRightBottom, playerCircle.transform.position, randomLineEndPos);
                intersectionPointCube.transform.position = intersectionPosition;
                if (isTooClose)
                {
                    isTooClose = false;
                }
            }
        }
        else
        {
            if (CrossCheck2D(mapLeftTop, mapLeftBottom, playerCircle.transform.position, randomLineEndPos))
            {
                intersectionPosition = GetCrossPosition(mapLeftTop, mapLeftBottom, playerCircle.transform.position, randomLineEndPos);
                intersectionPointCube.transform.position = intersectionPosition;
                if (isTooClose)
                {
                    isTooClose = false;
                }
            }
        }

        if(isTop)
        {
            if (CrossCheck2D(mapLeftTop, mapRightTop, playerCircle.transform.position, randomLineEndPos))
            {
                intersectionPosition = GetCrossPosition(mapLeftTop, mapRightTop, playerCircle.transform.position, randomLineEndPos);
                intersectionPointCube.transform.position = intersectionPosition;
                if (isTooClose)
                {
                    isTooClose = false;
                }
            }
        }
        else
        {
            if (CrossCheck2D(mapLeftBottom, mapRightBottom, playerCircle.transform.position, randomLineEndPos))
            {
                intersectionPosition = GetCrossPosition(mapLeftBottom, mapRightBottom, playerCircle.transform.position, randomLineEndPos);
                intersectionPointCube.transform.position = intersectionPosition;
                if (isTooClose)
                {
                    isTooClose = false;
                }
            }
        }
        float distance = Vector2.Distance(playerCircle.transform.position, intersectionPosition);

        if (Vector2.Distance(playerCircle.transform.position,intersectionPosition) <= CREATE_LIMIT_RANGE)
        {
            if(isTooClose)
            {
                Debug.Log("DoubleTooClose DirectionReset");
                isTooClose = false;
                SetRandomDirection();
            }
            else
            {
                Debug.Log("TooClose");

                isRight = !isRight;
                isTop = !isTop;
                isTooClose = true;

                randomLineEndPos.x *= -1;
                randomLineEndPos.y *= -1;

                ranLine.SetPosition(1, randomLineEndPos);
            }
        }
        
        

    }
    
    private void SetRandomDirection()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-10f, 10f), Random.Range(-10, 10));
        randomDirection.Normalize();

        isRight = randomDirection.x >= 0;
        isTop = randomDirection.y >= 0;

        Vector2 linePos = new Vector2(Random.Range(-9, 9), Random.Range(-9, 9));

        playerCircle.transform.position = linePos;
        randomLineEndPos = linePos + randomDirection * 30;

        ranLine.SetPosition(0, playerCircle.transform.position);
        ranLine.SetPosition(1, randomLineEndPos);

       // Debug.Log("RandomDirection.x = " + randomDirection.x + ", RandomDirection.y = " + randomDirection.y);
    }

    public bool CheckDotInLine(Vector2 _a, Vector2 _b, Vector3 _dot)
    {
        float epsilon = 0.00001f;
        float dAB = Vector2.Distance(_a, _b);
        float dADot = Vector2.Distance(_a, _dot);
        float dBDot = Vector2.Distance(_b, _dot);

        return ((dAB + epsilon) >= (dADot + dBDot));
    }

    public bool CrossCheck2D(Vector2 _a, Vector2 _b, Vector2 _c, Vector2 _d)
    {
        float x1, x2, x3, x4, y1, y2, y3, y4,X,Y;

        x1 = _a.x;
        y1 = _a.y;

        x2 = _b.x;
        y2 = _b.y;

        x3 = _c.x;
        y3 = _c.y;

        x4 = _d.x;
        y4 = _d.y;

        float isCross = ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

        X = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / isCross;
        Y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / isCross;

        if (isCross == 0)
        {
            return false;
        }
        else
        {
            return CheckDotInLine(_a, _b, new Vector2(X, Y)) && CheckDotInLine(_c, _d, new Vector2(X, Y));
        }

    }

    public Vector2 GetCrossPosition(Vector2 _a, Vector2 _b, Vector2 _c, Vector2 _d)
    {
        Vector2 result = Vector2.zero;

        float x1, x2, x3, x4, y1, y2, y3, y4;

        x1 = _a.x;
        y1 = _a.y;

        x2 = _b.x;
        y2 = _b.y;

        x3 = _c.x;
        y3 = _c.y;

        x4 = _d.x;
        y4 = _d.y;

        float isCross = ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

        if (isCross == 0)
        {
            return result;
        }
        else
        {
            result.x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / isCross;
            result.y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / isCross;

            return result;
        }
    }
}
